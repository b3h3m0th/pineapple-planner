namespace PineapplePlanner.UI.Pages
{
    public partial class Home
    {
        private List<Domain.Entities.Task> _tasks = new List<Domain.Entities.Task>();
        private string _error = string.Empty;

        protected override async System.Threading.Tasks.Task OnParametersSetAsync()
        {
            try
            {
                //_tasks = await _taskRepository.GetAllAsync();

                //await _taskRepository.AddAsync(new Domain.Entities.Task()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Name = "test",
                //    DateDue = DateTime.UtcNow,
                //    CreatedAt = DateTime.UtcNow,
                //});
            }
            catch (Exception e)
            {
                _error = e.Message + e.StackTrace;
            }

            await base.OnParametersSetAsync();
        }
    }
}
