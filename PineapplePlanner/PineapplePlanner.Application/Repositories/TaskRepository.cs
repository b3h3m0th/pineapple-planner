using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(FirestoreService firestoreService) : base(firestoreService)
    {
    }
}

