using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Layouts
{
    public partial class AuthenticatedLayout
    {
        [CascadingParameter(Name = "MainLayout")]
        public MainLayout? MainLayout { get; set; }

        private bool _isTaskDetailOpen;
        private Domain.Entities.Entry? _detailTask;
        public Domain.Entities.User? User { get; set; }

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

                if (userResult.IsSuccess && userResult.Data != null)
                {
                    User = userResult.Data;
                    MainLayout?.SetDarkMode(userResult.Data.IsDarkMode);
                    SetCulture(userResult.Data.Culture);
                }
            }
        }

        public void SetCulture(string name)
        {
            _localizationService.SetCulture(name);
            StateHasChanged();
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
