using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PineapplePlanner.UI.Layouts;

namespace PineapplePlanner.UI.Components
{
    public partial class Nav : IDisposable
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

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

        private void HandleCreateTask()
        {
            AuthenticatedLayout.OpenTaskDetail();
        }

        public void Dispose()
        {
            _authenticationStateProvider.AuthenticationStateChanged -= OnAuthStateChanged;
            GC.SuppressFinalize(this);
        }
    }
}
