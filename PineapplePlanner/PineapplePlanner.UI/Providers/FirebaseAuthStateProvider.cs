using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PineapplePlanner.UI.Providers
{
    public class FirebaseAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        private ClaimsPrincipal? _currentUser;

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
