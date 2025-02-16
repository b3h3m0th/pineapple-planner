using Microsoft.AspNetCore.Components;

namespace PineapplePlanner.UI.Components
{
    public partial class TaskItem
    {
        [Parameter]
        public Domain.Entities.Task Task { get; set; } = default!;

        [Parameter]
        public EventCallback<Domain.Entities.Task> OnClick { get; set; }

        [Parameter]
        public EventCallback<Domain.Entities.Task> OnCompleteChange { get; set; }

        private async Task HandleOnClick()
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(Task);
            }
        }

        private async Task HandleCompleteChange(ChangeEventArgs args)
        {
            if (OnCompleteChange.HasDelegate)
            {
                Task.CompletedAt = (bool?)args?.Value == true ? DateTime.UtcNow : null;
                await OnCompleteChange.InvokeAsync(Task);
            }
        }
    }
}
