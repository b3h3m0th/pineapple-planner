using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Layouts
{
    public partial class AuthenticatedLayout
    {
        [CascadingParameter(Name = "MainLayout")]
        public MainLayout? MainLayout { get; set; }

        private bool _isDarkMode;
        private bool _isTaskDetailOpen;
        private Domain.Entities.Entry? _detailTask;

        protected override async Task OnInitializedAsync()
        {
            await LoadUser();
            await base.OnInitializedAsync();
        }

        public async Task LoadUser()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                ResultBase<Domain.Entities.User?> userResult = await _userRepository.GetByUIdAsync(firebaseUid);
                _isDarkMode = userResult.Data?.IsDarkMode ?? false;

                if (userResult.IsSuccess && userResult.Data != null)
                {
                    MainLayout?.SetDarkMode(_isDarkMode);
                    MainLayout?.SetCulture(userResult.Data.Culture);
                }
            }
        }

        public void OpenTaskDetail(Domain.Entities.Entry? entry = null)
        {
            _detailTask = entry;
            _isTaskDetailOpen = true;

            StateHasChanged();
        }

        public void CloseTaskDetail()
        {
            _detailTask = null;
            _isTaskDetailOpen = false;

            StateHasChanged();
        }

        new public void StateHasChanged()
        {
            base.StateHasChanged();
        }
    }
}
