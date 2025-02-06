using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T>
{
    public BaseRepository(FirebaseService firestoreContext)
    {
    }

    public async Task<List<T>> GetAllAsync()
    {
        return new List<T>();
    }

    public async Task<object> GetAsync(T entity)
    {
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        return entity;
    }

    /// <inheritdoc />
    public async Task<T> UpdateAsync(T entity)
    {
        return entity;
    }

    /// <inheritdoc />
    public async Task<T> DeleteAsync(T entity)
    {
        return entity;
    }

    public async Task<List<T>> QueryRecordsAsync(object query)
    {
        return new List<T>();
    }
}

