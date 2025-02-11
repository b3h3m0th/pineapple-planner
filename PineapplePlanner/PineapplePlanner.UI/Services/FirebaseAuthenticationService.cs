using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Models.Auth;
using PineapplePlanner.UI.Providers;
using System.Security.Claims;

namespace PineapplePlanner.UI.Services
{
    public class FirebaseAuthenticationService : IDisposable
    {
        private readonly FirebaseAuth _auth;
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<FirebaseAuthenticationService>? _objRef;
        private readonly AuthenticationStateProvider _authProvider;

        public FirebaseAuthenticationService(IJSRuntime jsRuntime, AuthenticationStateProvider firebaseAuthStateProvider)
        {
            _jsRuntime = jsRuntime;
            _objRef = DotNetObjectReference.Create(this);
            _authProvider = firebaseAuthStateProvider;

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create();
            }

            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task<ResultBase<FirebaseUser>> LoginAsync(string email, string password)
        {
            ResultBase<FirebaseUser> result = ResultBase<FirebaseUser>.Success();

            try
            {
                SignInResult? signInResult = await _jsRuntime.InvokeAsync<SignInResult>("firebaseAuth.login", email, password);

                if (signInResult?.Success == true && !string.IsNullOrEmpty(signInResult.User?.Token))
                {
                    await ProcessAuthStateAsync(signInResult.User.Token);
                }
                else
                {
                    result.AddErrorAndSetFailure(signInResult?.Error ?? "");
                    ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsLoggedOut();
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        public async Task<ResultBase> LogoutAsync()
        {
            ResultBase result = ResultBase.Success();

            try
            {
                SignInResult? signInResult = await _jsRuntime.InvokeAsync<SignInResult>("firebaseAuth.logout");

                if (signInResult?.Success == true && !string.IsNullOrEmpty(signInResult.User?.Token))
                {
                    await ProcessAuthStateAsync(signInResult.User.Token);
                }
                else
                {
                    result.AddErrorAndSetFailure(signInResult?.Error ?? "");
                    ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsLoggedOut();
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        private async Task<ResultBase> ProcessAuthStateAsync(string idToken)
        {
            ResultBase<FirebaseUser> result = ResultBase<FirebaseUser>.Success();

            FirebaseToken decodedToken = await VerifyIdTokenAsync(idToken);

            if (decodedToken != null)
            {
                List<Claim> claims = [new Claim("FirebaseUid", decodedToken.Uid)];

                string? emailClaim = decodedToken.Claims["email"]?.ToString();

                if (!string.IsNullOrEmpty(emailClaim))
                {
                    claims.Add(new Claim(ClaimTypes.Email, emailClaim));
                }

                ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(claims));

                ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsAuthenticated(user);
            }
            else
            {
                ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsLoggedOut();
                result.IsSuccess = false;
            }

            return result;
        }

        [JSInvokable]
        public async Task OnAuthStateChangedAsync(string idToken)
        {
            await ProcessAuthStateAsync(idToken);
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            return await _auth.VerifyIdTokenAsync(idToken);
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}
