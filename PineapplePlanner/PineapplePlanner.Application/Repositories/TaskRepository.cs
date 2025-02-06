using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(FirestoreService firestoreService) : base(firestoreService)
    {

    }

    public async Task<List<Domain.Entities.Task>> GetAllAsync()
    {
        return new List<Domain.Entities.Task>()
        {
            new Domain.Entities.Task()
            {
                Id = "1",
                Name = "Task 1"
            },
            new Domain.Entities.Task()
            {
                Id = "2",
                Name = "Task 2"
            },
        };
    }
}

