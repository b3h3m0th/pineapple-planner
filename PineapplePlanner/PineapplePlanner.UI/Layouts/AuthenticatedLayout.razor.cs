namespace PineapplePlanner.UI.Layouts
{
    public partial class AuthenticatedLayout
    {
        private bool _isTaskDetailOpen = false;
        private Domain.Entities.Task? _detailTask;

        private void HandleCreateTask()
        {
            _detailTask = null;
            _isTaskDetailOpen = true;
        }
    }
}
