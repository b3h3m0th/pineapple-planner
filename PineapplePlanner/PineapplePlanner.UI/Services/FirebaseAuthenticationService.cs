using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PineapplePlanner.Domain.Dto.Authentication;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;
using System.Security.Claims;

namespace PineapplePlanner.UI.Services
{
    public class FirebaseAuthenticationService : IDisposable
    {
        private readonly FirebaseAuth _auth;
        private readonly IJSRuntime _jsRuntime;
        private readonly DotNetObjectReference<FirebaseAuthenticationService>? _objRef;
        private readonly AuthenticationStateProvider _authProvider;

        public FirebaseAuthenticationService(IJSRuntime jsRuntime, AuthenticationStateProvider firebaseAuthStateProvider)
        {
            _jsRuntime = jsRuntime;
            _objRef = DotNetObjectReference.Create(this);
            _authProvider = firebaseAuthStateProvider;

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-adminsdk-key.json")
                });
            }

            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task<ResultBase> LoginAsync(string email, string password)
        {
            ResultBase result = ResultBase.Success();

            try
            {
                LoginResultDto? loginResult = await _jsRuntime.InvokeAsync<LoginResultDto>("firebaseAuth.login", email, password);

                if (loginResult?.Success == true && !string.IsNullOrEmpty(loginResult.User?.Token))
                {
                    await ProcessAuthStateAsync(loginResult.User.Token);
                }
                else
                {
                    result.AddErrorAndSetFailure(loginResult?.Error ?? "");
                    ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsLoggedOut();
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        public async Task<ResultBase> RegisterAsync(string email, string password)
        {
            ResultBase result = ResultBase.Success();

            try
            {
                RegisterResultDto registerResult = await _jsRuntime.InvokeAsync<RegisterResultDto>("firebaseAuth.register", email, password);

                if (!string.IsNullOrEmpty(registerResult.Error))
                {
                    result.AddErrorAndSetFailure(registerResult.Error);
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
                LogoutResultDto? signInResult = await _jsRuntime.InvokeAsync<LogoutResultDto>("firebaseAuth.logout");

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

        public async Task<ResultBase> DeleteAsync()
        {
            ResultBase result = ResultBase.Success();

            try
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authProvider).FirebaseUid;
                if (string.IsNullOrEmpty(firebaseUid))
                {
                    result.SetFailure();
                }

                await LogoutAsync();
                await _auth.DeleteUserAsync(firebaseUid);
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        private async Task<ResultBase> ProcessAuthStateAsync(string idToken)
        {
            ResultBase<FirebaseUserDto> result = ResultBase<FirebaseUserDto>.Success();

            FirebaseToken decodedToken = await VerifyIdTokenAsync(idToken);

            if (decodedToken != null)
            {
                List<Claim> claims = [new Claim("FirebaseUid", decodedToken.Uid)];

                string? emailClaim = decodedToken.Claims["email"]?.ToString();

                if (!string.IsNullOrEmpty(emailClaim))
                {
                    claims.Add(new Claim(ClaimTypes.Email, emailClaim));
                }

                ClaimsPrincipal user = new(new ClaimsIdentity(claims, "firebase"));

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
            GC.SuppressFinalize(this);
        }
    }
}
