using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T> where T : IBaseFirestoreData
{
    private readonly FirestoreService _firestoreService;
    private readonly string _collectionName;

    public BaseRepository(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
        _collectionName = typeof(T).Name + "s";
    }

    public async Task AddAsync(T entity)
    {
        DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
        await docRef.SetAsync(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        QuerySnapshot snapshot = await _firestoreService.FirestoreDb.Collection(_collectionName).GetSnapshotAsync();
        return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(id);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        return snapshot.Exists ? snapshot.ConvertTo<T>() : null;
    }

    public async Task UpdateAsync(T entity)
    {
        DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
        await docRef.SetAsync(entity, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id)
    {
        DocumentReference docRef = _firestoreDb.Collection(_collectionName).Document(id);
        await docRef.DeleteAsync();
    }
}

