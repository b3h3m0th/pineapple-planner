﻿using Microsoft.AspNetCore.Components;
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
        private Domain.Entities.User? _user;
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
                Id = _userResult.Data?.Id ?? string.Empty,
                Username = _userResult.Data?.Username ?? string.Empty,
                Culture = _userResult.Data?.Culture ?? Culture.English,
                IsDarkMode = _userResult.Data?.IsDarkMode ?? false,
                UserUid = _userResult.Data?.UserUid ?? string.Empty,
            };
        }

        private async Task HandleSave()
        {
            ResultBase<Domain.Entities.User> user = new();

            if (!string.IsNullOrEmpty(_user?.Id))
            {
                user = await _userRepository.UpdateAsync(_user);
            }
            else
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

                if (!string.IsNullOrEmpty(firebaseUid) && _user != null)
                {
                    _user.UserUid = firebaseUid;
                    user = await _userRepository.CreateAsync(_user);
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

                Domain.Entities.User? changed = _user;

                return loaded.Username != changed?.Username || loaded.Culture != changed.Culture || loaded.IsDarkMode != changed.IsDarkMode;
            }
        }
    }
}
