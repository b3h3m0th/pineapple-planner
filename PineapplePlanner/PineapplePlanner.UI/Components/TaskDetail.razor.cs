using Microsoft.AspNetCore.Components;

namespace PineapplePlanner.UI.Components
{
    public partial class TaskDetail
    {
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
                Name = "",
                DateDue = DateTime.UtcNow,
            };

            await base.OnParametersSetAsync();
        }

        private void HandleDescriptionChange(ChangeEventArgs args)
        {
            Task.Description = args.Value?.ToString();
        }

        private void Close()
        {
            IsOpen = false;

            if (IsOpenChanged.HasDelegate)
            {
                IsOpenChanged.InvokeAsync(IsOpen);
            }
        }
    }
}
