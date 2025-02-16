using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(FirestoreService firestoreService) : base(firestoreService)
    {
    }

    public override Task<ResultBase<Domain.Entities.Task>> AddAsync(Domain.Entities.Task task)
    {
        task.CompletedAt = task.CompletedAt?.ToUniversalTime();
        task.CreatedAt = task.CreatedAt.ToUniversalTime();
        task.DeletedAt = task.DeletedAt?.ToUniversalTime();
        task.DateDue = task.DateDue.ToUniversalTime();

        return base.AddAsync(task);
    }

    public override Task<ResultBase<Domain.Entities.Task>> UpdateAsync(Domain.Entities.Task task)
    {
        task.CompletedAt = task.CompletedAt?.ToUniversalTime();
        task.CreatedAt = task.CreatedAt.ToUniversalTime();
        task.DeletedAt = task.DeletedAt?.ToUniversalTime();
        task.DateDue = task.DateDue.ToUniversalTime();

        return base.UpdateAsync(task);
    }
}

