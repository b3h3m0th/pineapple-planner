using Microsoft.AspNetCore.Components;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Components
{
    public partial class TaskDetail
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }

        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        [Parameter]
        public Domain.Entities.Task Task { get; set; } = default!;

        public bool IsCompleted { get; set; }

        public TimeSpan? DueTimeSpan
        {
            get => Task.DateDue?.TimeOfDay;
            set
            {
                if (Task.DateDue.HasValue)
                {
                    Task.DateDue = Task.DateDue.Value.Date + value;
                }
            }
        }

        public TimeSpan? StartDateTimeSpan
        {
            get => Task.StartDate?.TimeOfDay;
            set
            {
                if (Task.StartDate.HasValue)
                {
                    Task.StartDate = Task.StartDate.Value.Date + value;
                }
            }
        }

        public TimeSpan? EndDateTimeSpan
        {
            get => Task.EndDate?.TimeOfDay;
            set
            {
                if (Task.EndDate.HasValue)
                {
                    Task.EndDate = Task.EndDate.Value.Date + value;
                }
            }
        }

        private int _activeTabIndex = 0;

        protected override async Task OnParametersSetAsync()
        {
            Task ??= new Domain.Entities.Task()
            {
                Id = "",
                Name = "",
                DateDue = DateTime.Now,
            };

            IsCompleted = Task.CompletedAt != null;

            await base.OnParametersSetAsync();
        }

        private async Task HandleSave()
        {
            Task.CompletedAt = IsCompleted ? DateTime.Now : null;

            if (!string.IsNullOrEmpty(Task.Id))
            {
                await _taskRepository.UpdateAsync(Task);
            }
            else
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

                if (!string.IsNullOrEmpty(firebaseUid))
                {
                    Task.UserUid = firebaseUid;
                    await _taskRepository.AddAsync(Task);
                }
            }

            AuthenticatedLayout?.StateHasChanged();
        }

        private async Task HandleDelete()
        {
            await _taskRepository.DeleteAsync(Task.Id);

            AuthenticatedLayout?.StateHasChanged();
            HandleClose();
        }

        private void HandleClose()
        {
            IsOpen = false;

            if (IsOpenChanged.HasDelegate)
            {
                IsOpenChanged.InvokeAsync(IsOpen);
            }
        }
    }
}
