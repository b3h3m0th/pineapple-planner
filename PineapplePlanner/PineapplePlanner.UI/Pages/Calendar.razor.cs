using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Providers;
using System.Globalization;

namespace PineapplePlanner.UI.Pages
{
    public partial class Calendar
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [Parameter]
        public DateTime FirstDate { get; set; } = DateTime.Today;

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new ResultBase<List<Domain.Entities.Task>>();

        private string[] _dayNames = new CultureInfo("en").DateTimeFormat.AbbreviatedDayNames;

        protected override async Task OnParametersSetAsync()
        {
            await LoadTasks();

            await base.OnParametersSetAsync();
        }

        private async Task LoadTasks()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                _tasksResult = await _taskRepository.GetAllAsync(firebaseUid);
            }
        }

        private void HandleCreateTask(DateTime? day = null)
        {
            AuthenticatedLayout?.OpenTaskDetail(new Domain.Entities.Task()
            {
                Id = "",
                Name = "",
                DateDue = day,
                StartDate = day
            });
        }

        private void HandleEditTask(Domain.Entities.Task task)
        {
            AuthenticatedLayout?.OpenTaskDetail(task);
        }
    }
}
