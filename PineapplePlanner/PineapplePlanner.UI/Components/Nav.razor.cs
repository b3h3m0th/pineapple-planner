using Microsoft.AspNetCore.Components.Authorization;

namespace PineapplePlanner.UI.Components
{
    public partial class Nav : IDisposable
    {
        private AuthenticationState? _authenticationState;

        protected override async Task OnInitializedAsync()
        {
            _authenticationStateProvider.AuthenticationStateChanged += OnAuthStateChanged;
            _authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        private async void OnAuthStateChanged(Task<AuthenticationState> task)
        {
            _authenticationState = await task;
            StateHasChanged();
        }

        private async Task HandleLogout()
        {
            await _authenticationService.LogoutAsync();
        }

        public void Dispose()
        {
            _authenticationStateProvider.AuthenticationStateChanged -= OnAuthStateChanged;
        }
    }
}
