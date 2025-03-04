using Microsoft.AspNetCore.Components;
using MudBlazor;
using PineapplePlanner.Domain.Enums;

namespace PineapplePlanner.UI.Components
{
    public partial class TaskItem
    {
        [Parameter]
        public Domain.Entities.Task Task { get; set; } = default!;

        [Parameter]
        public EventCallback<Domain.Entities.Entry> OnClick { get; set; }

        [Parameter]
        public EventCallback<Domain.Entities.Entry> OnCompleteChange { get; set; }

        public bool IsCompleted { get => Task.CompletedAt != null; }
        public Color PriorityColor
        {
            get
            {
                return Task.Priority switch
                {
                    Priority.Low => Color.Info,
                    Priority.Medium => Color.Warning,
                    Priority.High => Color.Error,
                    _ => Color.Surface,
                };
            }
        }

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
