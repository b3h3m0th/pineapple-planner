﻿using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Enums;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T>
{
    private readonly Collection _collection;

    public BaseRepository(Collection collection)
    {
        // This should live in the appsetting file and injected - This is just an example.
        _collection = collection;
    }

    /// <inheritdoc />
    public async Task<List<T>> GetAllAsync()
    {
        var query = _firestoreDb.Collection(_collection.ToString());
        var querySnapshot = await query.GetSnapshotAsync();
        var list = new List<T>();
        foreach (var documentSnapshot in querySnapshot.Documents)
        {
            if (!documentSnapshot.Exists) continue;
            var data = documentSnapshot.ConvertTo<T>();
            if (data == null) continue;
            data.Id = documentSnapshot.Id;
            list.Add(data);
        }

        return list;
    }

    /// <inheritdoc />
    public async Task<object> GetAsync(T entity)
    {
        var docRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        var snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            var usr = snapshot.ConvertTo<T>();
            usr.Id = snapshot.Id;
            return usr;
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<T> AddAsync(T entity)
    {
        var colRef = _firestoreDb.Collection(_collection.ToString());
        var doc = await colRef.AddAsync(entity);
        // GO GET RECORD FROM DATABASE:
        // return (T) await GetAsync(entity); 
        return entity;
    }

    /// <inheritdoc />
    public async Task<T> UpdateAsync(T entity)
    {
        var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        await recordRef.SetAsync(entity, SetOptions.MergeAll);
        // GO GET RECORD FROM DATABASE:
        // return (T)await GetAsync(entity);
        return entity;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(T entity)
    {
        var recordRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        await recordRef.DeleteAsync();
    }

    public async Task<List<T>> QueryRecordsAsync(object query)
    {
        return new List<T>();
    }
}

