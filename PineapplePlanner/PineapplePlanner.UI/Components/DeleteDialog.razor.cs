using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PineapplePlanner.UI.Components
{
    public partial class DeleteDialog
    {
        [CascadingParameter]
        private IMudDialogInstance? MudDialog { get; set; }

        [Parameter]
        public string? ContentText { get; set; }

        [Parameter]
        public string? CancelText { get; set; }

        [Parameter]
        public string? SubmitText { get; set; }

        private void HandleSubmit() => MudDialog?.Close(DialogResult.Ok(true));

        private void HandleCancel() => MudDialog?.Cancel();
    }
}
