using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.UI.Pages
{
    public partial class Login
    {
        private string _error = "";
        private string _message = "";

        private string _email = "";
        private string _password = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            ResultBase<(string email, string password)?> credentialsResult = _credentialsService.Get();

            if (credentialsResult.IsSuccess && credentialsResult.Data != null)
            {
                _email = credentialsResult.Data?.email ?? string.Empty;
                _password = credentialsResult.Data?.password ?? string.Empty;
            }

            if (!string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_password))
            {
                await HandleLogin();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task HandleLogin()
        {
            ResultBase result = await _authService.LoginAsync(_email, _password);

            if (result.IsSuccess)
            {
                _credentialsService.Save(_email, _password);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                _credentialsService.Remove();
                _error = string.Join(", ", result.Errors);
            }
        }
    }
}

