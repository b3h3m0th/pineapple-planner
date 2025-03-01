using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
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
        private string _addATag = string.Empty;

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

        private void HandleAddTag(string tag)
        {
            if (string.IsNullOrEmpty(tag.Trim())) return;

            Task.Tags.Add(new Domain.Entities.Tag()
            {
                Id = Guid.NewGuid().ToString(),
                Name = tag
            });
            StateHasChanged();
            _addATag = string.Empty;
        }

        private void HandleRemoveTag(MudChip<Domain.Entities.Tag> chip)
        {
            if (chip.Value == null) return;

            Domain.Entities.Tag? toRemove = Task.Tags.Find(t => t.Id == chip.Value.Id);
            if (toRemove != null)
            {
                Task.Tags.Remove(toRemove);
            }

            StateHasChanged();
        }

        private void HandleAddATagKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                HandleAddTag(_addATag);
            }

            StateHasChanged();
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
