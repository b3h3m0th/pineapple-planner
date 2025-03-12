using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Localization;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Pages
{
    public partial class Settings
    {
        private enum SettingsTab
        {
            Profile = 1,
            Appearance
        }

        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [CascadingParameter(Name = "MainLayout")]
        public MainLayout? MainLayout { get; set; }

        private ResultBase<Domain.Entities.User?> _userResult = new();
        private Domain.Entities.User _user = new()
        {
            Id = string.Empty,
            Username = string.Empty,
            Culture = Culture.English,
            UserUid = string.Empty
        };

        private int _activeTabIndex = (int)SettingsTab.Profile;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadUser();
            await base.OnParametersSetAsync();
        }

        private async Task LoadUser()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                _userResult = await _userRepository.GetByUIdAsync(firebaseUid);
            }

            if (_userResult.IsSuccess && _userResult.Data != null)
            {
                _user = new()
                {
                    Id = _userResult.Data.Id,
                    Username = _userResult.Data.Username,
                    Culture = _userResult.Data.Culture,
                    IsDarkMode = _userResult.Data.IsDarkMode,
                    UserUid = _userResult.Data.UserUid,
                };
            }
            else
            {
                _user = new()
                {
                    Id = string.Empty,
                    UserUid = string.Empty,
                    Username = string.Empty,
                    Culture = Culture.English
                };
            }
        }

        private async Task HandleSave()
        {
            ResultBase<Domain.Entities.User> user = new();

            if (!string.IsNullOrEmpty(_user.Id))
            {
                user = await _userRepository.UpdateAsync(_user);
            }
            else
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

                if (!string.IsNullOrEmpty(firebaseUid))
                {
                    _user.UserUid = firebaseUid;
                    user = await _userRepository.CreateAsync(_user);
                }
            }

            if (user.IsSuccess && user.Data != null && AuthenticatedLayout != null)
            {
                AuthenticatedLayout?.SetCulture(user.Data.Culture);
                MainLayout?.SetDarkMode(user.Data.IsDarkMode);
            }
        }

        private async Task HandleCancel()
        {
            await LoadUser();
        }

        private async Task HandleDeleteAccount()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;
            if (string.IsNullOrEmpty(firebaseUid)) return;

            await _entryRepository.DeleteAllAsync(firebaseUid);
            await _userRepository.DeleteAsync(firebaseUid);
            await _authService.DeleteAsync();
            _navigationManager.NavigateTo("/");
        }

        private bool HasChanges
        {
            get
            {
                Domain.Entities.User? loaded = _userResult.Data;
                if (loaded == null) return false;

                Domain.Entities.User changed = _user;

                return loaded.Username != changed.Username || loaded.Culture != changed.Culture || loaded.IsDarkMode != changed.IsDarkMode;
            }
        }
    }
}
