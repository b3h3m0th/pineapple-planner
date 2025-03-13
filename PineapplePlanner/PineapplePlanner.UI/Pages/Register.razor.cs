using FirebaseAdmin.Auth;
using Microsoft.JSInterop;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;
using System.Security.Claims;

namespace PineapplePlanner.UI.Pages
{
    public partial class Register
    {
        private string _error = "";
        private string _message = "";

        private string _email = "";
        private string _password = "";

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
                _message = "logged in";
            }
            else
            {
                _message = "logged out";
                ((FirebaseAuthStateProvider)_authenticationProvider)?.MarkUserAsLoggedOut();
            }
        }

        private async Task HandleRegister()
        {
            ResultBase result = await _authenticationService.RegisterAsync(_email, _password);

            if (result.IsSuccess)
            {
                _navigationManager.NavigateTo("/");
            }
            else
            {
                _error = string.Join(", ", result.Errors);
            }

            StateHasChanged();
        }
    }
}
