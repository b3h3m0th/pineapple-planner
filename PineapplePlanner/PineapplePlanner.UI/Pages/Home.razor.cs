using PineapplePlanner.Application;

namespace PineapplePlanner.UI.Pages
{
    public partial class Home
    {
        private GetTodos.Result _todosResult = new GetTodos.Result();
        private List<Domain.Entities.Task> _tasks = new List<Domain.Entities.Task>();
        private string _error = string.Empty;

        protected override async System.Threading.Tasks.Task OnParametersSetAsync()
        {
            try
            {
                _todosResult = await _mediator.Send(new GetTodos.Query());
                //_tasks = await _taskRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                _error = e.Message;
            }

            await base.OnParametersSetAsync();
        }
    }
}
