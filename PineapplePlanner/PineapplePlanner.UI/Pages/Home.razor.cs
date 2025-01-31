using PineapplePlanner.Application;

namespace PineapplePlanner.UI.Pages
{
    public partial class Home
    {
        private GetTodos.Result _todosResult = new GetTodos.Result();
        private string _error = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                _todosResult = await _mediator.Send(new GetTodos.Query());
            }
            catch (Exception e)
            {
                _error = e.Message;
            }

            await base.OnParametersSetAsync();
        }
    }
}
