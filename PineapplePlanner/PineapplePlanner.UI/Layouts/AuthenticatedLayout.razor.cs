namespace PineapplePlanner.UI.Layouts
{
    public partial class AuthenticatedLayout
    {
        private bool _isTaskDetailOpen = false;
        private Domain.Entities.Entry? _detailTask;

        public void OpenTaskDetail(Domain.Entities.Entry? task = null)
        {
            _detailTask = task;
            _isTaskDetailOpen = true;

            StateHasChanged();
        }

        public void CloseTaskDetail()
        {
            _detailTask = null;
            _isTaskDetailOpen = false;

            StateHasChanged();
        }

        new public void StateHasChanged()
        {
            base.StateHasChanged();
        }
    }
}
