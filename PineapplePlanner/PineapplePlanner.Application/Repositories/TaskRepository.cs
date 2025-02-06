using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Entities;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
{
    public TaskRepository(FirestoreService firestoreService) : base(firestoreService)
    {

    }

    public async Task<List<Task>> GetAllAsync()
    {
        QuerySnapshot snapshot = await _firestoreService.FirestoreDb.Collection(_collectionName).GetSnapshotAsync();
        return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
    }
}

