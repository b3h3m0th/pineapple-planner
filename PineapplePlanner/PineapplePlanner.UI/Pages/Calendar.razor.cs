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

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new();

        private readonly string[] _dayNames = new CultureInfo("en").DateTimeFormat.AbbreviatedDayNames;

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

        private bool TaskOverlapsDay(Domain.Entities.Task task, DateTime day)
        {
            DateTime taskStart = task.StartDate ?? DateTime.MinValue;
            DateTime taskEnd = task.EndDate ?? DateTime.MaxValue;
            DateTime dayEnd = day.AddDays(1);

            return taskStart < dayEnd && taskEnd > day;
        }

        private string GetTaskStyle(Domain.Entities.Task task, DateTime currentDate)
        {
            DateTime taskStart = task.StartDate ?? DateTime.MinValue;
            DateTime taskEnd = task.EndDate ?? DateTime.MaxValue;

            DateTime dayStart = currentDate;
            DateTime dayEnd = currentDate.AddDays(1);

            DateTime displayStart = (taskStart > dayStart) ? taskStart : dayStart;
            DateTime displayEnd = (taskEnd < dayEnd) ? taskEnd : dayEnd;

            double offsetY = (displayStart - dayStart).TotalMinutes;
            double height = (displayEnd - displayStart).TotalMinutes;

            return $"margin-top: {offsetY.ToString("0")}px !important; height: {height.ToString("0")}px;";
        }
    }
}
