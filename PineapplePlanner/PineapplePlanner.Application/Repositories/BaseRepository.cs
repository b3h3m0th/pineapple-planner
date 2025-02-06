using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Application.Repositories;

public class BaseRepository<T> : IBaseRespository<T> where T : IBaseFirestoreData
{
    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
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

