using Microsoft.AspNetCore.Components;
using MudBlazor;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Components;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Localization;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Pages
{
    public partial class Settings
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [CascadingParameter(Name = "MainLayout")]
        public MainLayout? MainLayout { get; set; }

        private ResultBase<Domain.Entities.User?> _userResult = new();
        private Domain.Entities.User? _user;
        private readonly Domain.Entities.User _defaultUser = new()
        {
            Id = string.Empty,
            Username = string.Empty,
            Culture = Culture.English,
            IsDarkMode = false,
            UserUid = string.Empty,
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

            _user = new()
            {
                Id = _userResult.Data?.Id ?? _defaultUser.Id,
                Username = _userResult.Data?.Username ?? _defaultUser.Username,
                Culture = _userResult.Data?.Culture ?? _defaultUser.Culture,
                IsDarkMode = _userResult.Data?.IsDarkMode ?? _defaultUser.IsDarkMode,
                UserUid = _userResult.Data?.UserUid ?? _defaultUser.UserUid,
            };
        }

        private async Task HandleSave()
        {
            if (!string.IsNullOrEmpty(_user?.Id))
            {
                await _userRepository.UpdateAsync(_user);
            }
            else
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

                if (!string.IsNullOrEmpty(firebaseUid) && _user != null)
                {
                    _user.UserUid = firebaseUid;
                    await _userRepository.CreateAsync(_user);
                }
            }

            AuthenticatedLayout?.LoadUser();
        }

        private async Task HandleCancel()
        {
            await LoadUser();
        }

        private async Task HandleDeleteAccount()
        {
            IDialogReference dialogReference = await _dialogService.ShowAsync<DeleteDialog>(_localize["Delete account"], new DialogParameters<DeleteDialog>()
            {
                { x => x.SubmitText, _localize["Delete"] },
                { x => x.CancelText, _localize["Cancel"] },
                { x => x.ContentText, _localize["This action cannot be undone"] },
            });
            DialogResult? dialogResult = await dialogReference.Result;

            if (dialogResult?.Canceled == true) return;

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
                Domain.Entities.User? changed = _user;

                if (loaded == null)
                {
                    return changed != null && (changed.Username != _defaultUser.Username || changed.Culture != _defaultUser.Culture || changed.IsDarkMode != _defaultUser.IsDarkMode);
                }

                return loaded?.Username != changed?.Username || loaded?.Culture != changed?.Culture || loaded?.IsDarkMode != changed?.IsDarkMode;
            }
        }
    }
}
