﻿using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.UI.Pages
{
    public partial class Login
    {
        private string _error = "";
        private string _message = "";

        private string _email = "simonostini@gmail.com";
        private string _password = "test123";

        private async Task HandleLogin()
        {
            ResultBase result = await _authService.LoginAsync(_email, _password);

            if (result.IsSuccess)
            {
                _navigationManager.NavigateTo("/");
            }
            else
            {
                _error = string.Join(", ", result.Errors);
            }
        }
    }
}

