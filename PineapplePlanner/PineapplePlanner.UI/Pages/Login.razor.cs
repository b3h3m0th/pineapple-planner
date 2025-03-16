using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.UI.Pages
{
    public partial class Login
    {
        private string _error = "";
        private string _email = "";
        private string _password = "";
        private bool _isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
                else
                {
                    _isLoading = false;
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task HandleLogin()
        {
            _isLoading = true;
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
            _isLoading = false;
        }
    }
}

