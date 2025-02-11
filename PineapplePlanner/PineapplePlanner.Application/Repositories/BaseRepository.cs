using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T> where T : IBaseFirestoreData
{
    protected readonly FirestoreService _firestoreService;
    protected readonly string _collectionName;

    public BaseRepository(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
        _collectionName = typeof(T).Name + "s";
    }

    public async Task<ResultBase<List<T>>> GetAllAsync()
    {
        var result = ResultBase<List<T>>.Success();

        try
        {
            QuerySnapshot snapshot = await _firestoreService.FirestoreDb.Collection(_collectionName).GetSnapshotAsync();
            var documents = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();

            return new ResultBase<List<T>>(documents);
        }
        catch (Exception ex)
        {
            result.AddErrorAndSetFailure(ex.Message + ex.StackTrace);
        }

        return result;
    }

    public async Task<ResultBase<T?>> GetByIdAsync(string id)
    {
        try
        {
            DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            var document = snapshot.Exists ? snapshot.ConvertTo<T>() : default;

            return ResultBase<T?>.Success(document);
        }
        catch (Exception)
        {
            return ResultBase<T?>.Failure();
        }
    }

    public async Task<ResultBase> AddAsync(T entity)
    {
        try
        {
            DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
            await docRef.SetAsync(entity);

            return ResultBase.Success();
        }
        catch (Exception)
        {
            return ResultBase.Failure();
        }
    }

    public async Task<ResultBase> UpdateAsync(T entity)
    {
        try
        {
            DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
            await docRef.SetAsync(entity, SetOptions.Overwrite);

            return ResultBase.Success();
        }
        catch (Exception)
        {
            return ResultBase.Failure();
        }
    }

    public async Task<ResultBase> DeleteAsync(string id)
    {
        try
        {
            DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(id);
            await docRef.DeleteAsync();

            return ResultBase.Success();
        }
        catch (Exception)
        {
            return ResultBase.Failure();
        }
    }
}

