using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PineapplePlanner.UI.Providers
{
    public class FirebaseAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private ClaimsPrincipal? _currentUser;

        public string? FirebaseUid
        {
            get
            {
                return GetAuthenticationStateAsync().Result.User.Claims.FirstOrDefault(c => c.Type == "FirebaseUid")?.Value;
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser ?? _anonymous));
        }

        public void MarkUserAsAuthenticated(ClaimsPrincipal user)
        {
            _currentUser = user;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void MarkUserAsLoggedOut()
        {
            _currentUser = _anonymous;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
