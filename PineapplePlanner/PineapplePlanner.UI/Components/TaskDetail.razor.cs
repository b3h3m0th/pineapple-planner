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
        public Domain.Entities.Entry Entry { get; set; } = default!;

        public TimeSpan? DueTimeSpan
        {
            get => (Entry as Domain.Entities.Task)?.DateDue?.TimeOfDay;
            set
            {
                if (Entry is Domain.Entities.Task task)
                {
                    if (task.DateDue.HasValue)
                    {
                        task.DateDue = task.DateDue.Value.Date + value;
                    }
                }
            }
        }

        public TimeSpan? StartDateTimeSpan
        {
            get => (Entry as Domain.Entities.Event)?.StartDate?.TimeOfDay;
            set
            {
                if (Entry is Domain.Entities.Event entryEvent)
                {
                    if (entryEvent.StartDate.HasValue)
                    {
                        entryEvent.StartDate = entryEvent.StartDate.Value.Date + value;
                    }
                }
            }
        }

        public TimeSpan? EndDateTimeSpan
        {
            get => (Entry as Domain.Entities.Event)?.EndDate?.TimeOfDay;
            set
            {
                if (Entry is Domain.Entities.Event entryEvent)
                {
                    if (entryEvent.EndDate.HasValue)
                    {
                        entryEvent.EndDate = entryEvent.EndDate.Value.Date + value;
                    }
                }
            }
        }

        private string _addATag = string.Empty;
        private int _activeTabIndex = 0;
        private bool _isCompleted;

        protected override async Task OnParametersSetAsync()
        {
            Entry ??= new Domain.Entities.Task()
            {
                Id = "",
                Name = "",
                DateDue = DateTime.Now
            };

            _activeTabIndex = Entry is Domain.Entities.Task ? 0 : 1;
            _isCompleted = (Entry as Domain.Entities.Task)?.IsCompleted ?? false;

            await base.OnParametersSetAsync();
        }

        private void HandleAddTag(string tag)
        {
            if (string.IsNullOrEmpty(tag.Trim())) return;

            Entry.Tags.Add(new Domain.Entities.Tag()
            {
                Id = Guid.NewGuid().ToString(),
                Name = tag
            });
            _addATag = string.Empty;
        }

        private void HandleRemoveTag(MudChip<Domain.Entities.Tag> chip)
        {
            if (chip.Value == null) return;

            Domain.Entities.Tag? toRemove = Entry.Tags.Find(t => t.Id == chip.Value.Id);
            if (toRemove != null)
            {
                Entry.Tags.Remove(toRemove);
            }

            StateHasChanged();
        }

        private void HandleAddATagKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                HandleAddTag(_addATag);
            }
        }

        private void HandleTabChange()
        {
            if (_activeTabIndex == 0 && Entry is Domain.Entities.Event eventEntry)
            {
                Entry = new Domain.Entities.Task()
                {
                    Id = eventEntry.Id,
                    Name = eventEntry.Name,
                    Description = eventEntry.Description,
                    Priority = eventEntry.Priority,
                    Tags = eventEntry.Tags,
                    UserUid = eventEntry.UserUid,
                    DateDue = eventEntry.StartDate
                };
            }
            else if (_activeTabIndex == 1 && Entry is Domain.Entities.Task task)
            {
                Entry = new Domain.Entities.Event()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Priority = task.Priority,
                    Tags = task.Tags,
                    UserUid = task.UserUid,
                    StartDate = task.DateDue,
                    EndDate = task.DateDue?.AddMinutes(30)
                };
            }
        }

        private async Task HandleSave()
        {
            if (Entry is Domain.Entities.Task task)
            {
                task.CompletedAt = _isCompleted ? DateTime.Now : null;
            }

            if (!string.IsNullOrEmpty(Entry.Id))
            {
                await _taskRepository.UpdateAsync(Entry);
            }
            else
            {
                string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

                if (!string.IsNullOrEmpty(firebaseUid))
                {
                    Entry.UserUid = firebaseUid;
                    await _taskRepository.AddAsync(Entry);
                }
            }

            AuthenticatedLayout?.StateHasChanged();
        }

        private async Task HandleDelete()
        {
            await _taskRepository.DeleteAsync(Entry.Id);

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
