﻿using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class EntryRepository(FirestoreService firestoreService) : BaseRepository<Domain.Entities.Entry>(firestoreService), IEntryRepository
{
    public override Task<ResultBase<Domain.Entities.Entry>> AddAsync(Domain.Entities.Entry entry)
    {
        entry.CreatedAt = DateTime.Now.ToUniversalTime();
        entry.DeletedAt = entry.DeletedAt?.ToUniversalTime();

        if (entry is Domain.Entities.Task task)
        {
            task.DateDue = task.DateDue?.ToUniversalTime();
            task.CompletedAt = task.CompletedAt?.ToUniversalTime();
        }

        if (entry is Domain.Entities.Event eventEntry)
        {
            eventEntry.StartDate = eventEntry.StartDate?.ToUniversalTime();
            eventEntry.EndDate = eventEntry.EndDate?.ToUniversalTime();
        }

        return base.AddAsync(entry);
    }

    public override Task<ResultBase<Domain.Entities.Entry>> UpdateAsync(Domain.Entities.Entry entry)
    {
        entry.CreatedAt = entry.CreatedAt.ToUniversalTime();
        entry.DeletedAt = entry.DeletedAt?.ToUniversalTime();

        if (entry is Domain.Entities.Task task)
        {
            task.DateDue = task.DateDue?.ToUniversalTime();
            task.CompletedAt = task.CompletedAt?.ToUniversalTime();
        }

        if (entry is Domain.Entities.Event eventEntry)
        {
            eventEntry.StartDate = eventEntry.StartDate?.ToUniversalTime();
            eventEntry.EndDate = eventEntry.EndDate?.ToUniversalTime();
        }

        return base.UpdateAsync(entry);
    }

    public async Task<ResultBase<List<T>>> GetAllAsync<T>(string userId) where T : Domain.Entities.Entry
    {
        ResultBase<List<T>> result = ResultBase<List<T>>.Success();

        try
        {
            QuerySnapshot snapshot = await _firestoreService.FirestoreDb
                .Collection(_collectionName)
                .WhereEqualTo("UserUid", userId)
                .WhereEqualTo("Type", typeof(T).Name)
                .GetSnapshotAsync();
            List<T> documents = [.. snapshot.Documents.Select(doc => doc.ConvertTo<T>())];

            return new ResultBase<List<T>>(documents);
        }
        catch (Exception ex)
        {
            result.AddErrorAndSetFailure(ex.Message + ex.StackTrace);
        }

        return result;
    }

    public async Task<ResultBase<List<Domain.Entities.Entry>>> DeleteAllAsync(string userId)
    {
        ResultBase<List<Domain.Entities.Entry>> result = ResultBase<List<Domain.Entities.Entry>>.Success();

        try
        {
            QuerySnapshot snapshot = await _firestoreService.FirestoreDb
                .Collection(_collectionName)
                .WhereEqualTo("UserUid", userId)
                .GetSnapshotAsync();

            List<Domain.Entities.Entry> documents = [.. snapshot.Documents.Select(doc => doc.ConvertTo<Domain.Entities.Entry>())];

            await Task.WhenAll(snapshot.Documents.Select(d => d.Reference.DeleteAsync()));

            result.Data = documents;
        }
        catch (Exception ex)
        {
            result.AddErrorAndSetFailure(ex.Message + ex.StackTrace);
        }

        return result;
    }
}

