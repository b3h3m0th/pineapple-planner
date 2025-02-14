using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;

namespace PineapplePlanner.UI.Pages
{
    public partial class Home
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new ResultBase<List<Domain.Entities.Task>>();

        protected override async System.Threading.Tasks.Task OnParametersSetAsync()
        {
            //await _taskRepository.AddAsync(new Domain.Entities.Task()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "test",
            //    DateDue = DateTime.UtcNow,
            //    CreatedAt = DateTime.UtcNow,
            //    Priority = Domain.Enums.Priority.High,
            //    Tags = new List<Tag>()
            //    {
            //        new Tag()
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            Name = "tag1",
            //        }
            //    }
            //});

            _tasksResult = await _taskRepository.GetAllAsync();

            await base.OnParametersSetAsync();
        }

        private void HandleEditTask(Domain.Entities.Task task)
        {
            AuthenticatedLayout?.OpenTaskDetail(task);
        }
    }
}
