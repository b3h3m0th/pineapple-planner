using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T> where T : IBaseFirestoreData
{
  protected readonly string _collectionName;
  protected readonly FirestoreService _firestoreService;

  public BaseRepository(FirestoreService firestoreService)
  {
    _firestoreService = firestoreService;
    _collectionName = typeof(T).Name + "s";
  }

  public async Task<ResultBase<List<T>>> GetAllAsync()
  {
    ResultBase<List<T>> result = ResultBase<List<T>>.Success();

    try
    {
      QuerySnapshot snapshot = await _firestoreService.FirestoreDb
          .Collection(_collectionName)
          .GetSnapshotAsync();
      List<T> documents = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();

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
      T? document = snapshot.Exists ? snapshot.ConvertTo<T>() : default;

      return ResultBase<T?>.Success(document);
    }
    catch (Exception)
    {
      return ResultBase<T?>.Failure();
    }
  }

  public virtual async Task<ResultBase<T>> AddAsync(T entity)
  {
    ResultBase<T> result = ResultBase<T>.Success();

    try
    {
      entity.Id = Guid.NewGuid().ToString();
      DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
      await docRef.SetAsync(entity);

      result.Data = entity;
    }
    catch (Exception ex)
    {
      result.AddErrorAndSetFailure(ex.Message);
    }

    return result;
  }

  public virtual async Task<ResultBase<T>> UpdateAsync(T entity)
  {
    ResultBase<T> result = ResultBase<T>.Success();

    try
    {
      DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(entity.Id);
      await docRef.SetAsync(entity, SetOptions.Overwrite);

      result.Data = entity;
    }
    catch (Exception ex)
    {
      result.AddErrorAndSetFailure(ex.Message);
    }

    return result;
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
