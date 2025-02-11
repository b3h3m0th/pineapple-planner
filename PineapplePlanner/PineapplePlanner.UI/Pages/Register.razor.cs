using Microsoft.JSInterop;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Models.Auth;
using PineapplePlanner.UI.Providers;
using System.Security.Claims;

namespace PineapplePlanner.UI.Pages
{
    public partial class Register
    {
        private string _error = "";
        private string _message = "";

        private string _email = string.Empty;
        private string _password = string.Empty;

        [JSInvokable]
        public async Task OnAuthStateChanged(string idToken)
        {
            if (!string.IsNullOrEmpty(idToken))
            {
                var decodedToken = await _authService.VerifyIdTokenAsync(idToken);
                var user = new ClaimsPrincipal(new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, decodedToken.Claims["name"].ToString() ?? ""),
                    new Claim(ClaimTypes.Email, decodedToken.Claims["email"].ToString() ?? ""),
                    new Claim("FirebaseUid", decodedToken.Uid)
                ], "firebase"));

                ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsAuthenticated(user);
                _message = "logged in";
            }
            else
            {
                _message = "logged out";
                ((FirebaseAuthStateProvider)_authProvider)?.MarkUserAsLoggedOut();
            }
        }

        private async Task HandleRegister()
        {
            try
            {
                ResultBase<FirebaseUser> result = await _authService.LoginAsync(_email, _password);
                _message = (result.Errors.Any()).ToString();

                if (result.Errors.Any())
                {
                    _error += result.Errors[0];
                }
            }
            catch (Exception ex)
            {
                _error += ex.Message + ex.StackTrace;
            }

            StateHasChanged();
        }
    }
}
