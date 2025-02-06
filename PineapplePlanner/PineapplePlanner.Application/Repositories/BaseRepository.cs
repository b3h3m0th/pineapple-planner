using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T> where T : IBaseFirestoreData
{
    private readonly FirestoreService _firestoreService;

    public BaseRepository(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
    }

    public async Task AddAsync(T entity)
    {
        DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
        await docRef.SetAsync(entity);
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}

