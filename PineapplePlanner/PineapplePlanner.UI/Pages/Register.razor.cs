using FirebaseAdmin.Auth;
using Microsoft.JSInterop;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;
using System.Security.Claims;

namespace PineapplePlanner.UI.Pages
{
    public partial class Register
    {
        private string _error = string.Empty;
        private string _message = string.Empty;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        private bool _isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isLoading = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task HandleRegister()
        {
            _isLoading = true;
            if (!IsPasswordValid())
            {
                _error = "Passwords do not match.";
            }
            else
            {
                ResultBase result = await _authenticationService.RegisterAsync(_email, _password);

                if (result.IsSuccess)
                {
                    _message = _localize["Verification email sent."];
                }
                else
                {
                    _error = string.Join(", ", result.Errors);
                }
            }

            _isLoading = false;
            StateHasChanged();
        }

        private bool IsPasswordValid()
        {
            return _password == _confirmPassword;
        }

        [JSInvokable]
        public async Task OnAuthStateChanged(string idToken)
        {
            if (!string.IsNullOrEmpty(idToken))
            {
                FirebaseToken decodedToken = await _authenticationService.VerifyIdTokenAsync(idToken);
                var user = new ClaimsPrincipal(new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, decodedToken.Claims["name"].ToString() ?? ""),
                    new Claim(ClaimTypes.Email, decodedToken.Claims["email"].ToString() ?? ""),
                    new Claim("FirebaseUid", decodedToken.Uid)
                ], "firebase"));

                ((FirebaseAuthStateProvider)_authenticationProvider)?.MarkUserAsAuthenticated(user);
            }
            else
            {
                ((FirebaseAuthStateProvider)_authenticationProvider)?.MarkUserAsLoggedOut();
            }
        }
    }
}
