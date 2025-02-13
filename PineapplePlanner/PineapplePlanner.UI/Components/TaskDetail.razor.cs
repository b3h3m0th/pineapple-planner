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
        public Domain.Entities.Task? Task { get; set; }

    }
}
