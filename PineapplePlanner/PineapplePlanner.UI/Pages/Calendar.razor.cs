using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Pages
{
    public partial class Calendar
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [Parameter]
        public DateTime FirstDate { get; set; } = DateTime.Today;

        private ResultBase<List<Domain.Entities.Event>> _eventsResult = new();

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
                _eventsResult = await _taskRepository.GetAllAsync<Domain.Entities.Event>(firebaseUid);
            }
        }

        private void HandleCreateTask(DateTime? date = null)
        {
            AuthenticatedLayout?.OpenTaskDetail(new Domain.Entities.Event()
            {
                Id = "",
                Name = "",
                StartDate = date,
                EndDate = date?.AddMinutes(30)
            });
        }

        private void HandleEditTask(Domain.Entities.Event eventEntry)
        {
            AuthenticatedLayout?.OpenTaskDetail(eventEntry);
        }

        private void HandleNavigateToWeek(DateTime firstDate)
        {
            FirstDate = firstDate;
        }

        private static bool TaskOverlapsDay(Domain.Entities.Event eventEntry, DateTime day)
        {
            DateTime taskStart = eventEntry.StartDate ?? DateTime.MinValue;
            DateTime taskEnd = eventEntry.EndDate ?? DateTime.MaxValue;
            DateTime dayEnd = day.AddDays(1);

            return taskStart < dayEnd && taskEnd > day;
        }

        private static bool TaskOverlapsHour(Domain.Entities.Event eventEntry, DateTime hour)
        {
            DateTime taskStart = eventEntry.StartDate ?? DateTime.MinValue;
            DateTime taskEnd = eventEntry.EndDate ?? DateTime.MaxValue;
            DateTime dayEnd = hour.AddHours(1);

            return taskStart < dayEnd && taskEnd > hour;
        }

        private static string GetTaskStyle(Domain.Entities.Event eventEntry, DateTime currentDate)
        {
            DateTime taskStart = eventEntry.StartDate ?? DateTime.MinValue;
            DateTime taskEnd = eventEntry.EndDate ?? DateTime.MaxValue;

            DateTime dayStart = currentDate;
            DateTime dayEnd = currentDate.AddDays(1);

            DateTime displayStart = (taskStart > dayStart) ? taskStart : dayStart;
            DateTime displayEnd = (taskEnd < dayEnd) ? taskEnd : dayEnd;

            double offsetY = (displayStart - dayStart).TotalMinutes;
            double height = (displayEnd - displayStart).TotalMinutes;

            return $"margin-top: {offsetY:0}px !important; height: {height:0}px;";
        }
    }
}
