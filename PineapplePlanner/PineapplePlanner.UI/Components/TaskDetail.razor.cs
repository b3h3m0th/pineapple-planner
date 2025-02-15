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

        protected override async Task OnParametersSetAsync()
        {
            Task ??= new Domain.Entities.Task()
            {
                Id = "",
                Name = "",
                DateDue = DateTime.UtcNow,
            };

            await base.OnParametersSetAsync();
        }

        private void HandleDescriptionChange(ChangeEventArgs args)
        {
            Task.Description = args.Value?.ToString();
        }

        private async Task HandleSave()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                Task.UserUid = firebaseUid;
                await _taskRepository.AddAsync(Task);
                AuthenticatedLayout?.StateHasChanged();
            }
        }

        private async Task HandleDelete()
        {
            await _taskRepository.DeleteAsync(Task.Id);
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
