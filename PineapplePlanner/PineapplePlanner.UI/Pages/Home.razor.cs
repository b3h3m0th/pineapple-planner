﻿using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.UI.Pages
{
    public partial class Home
    {
        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new ResultBase<List<Domain.Entities.Task>>();
        private Domain.Entities.Task? _detailTask;
        private bool _isDetailOpen;

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
            _isDetailOpen = true;
            _detailTask = task;
        }
    }
}
