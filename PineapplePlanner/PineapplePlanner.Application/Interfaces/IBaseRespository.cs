using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IBaseRespository<T> where T : IBaseFirestoreData
    {
        Task<ResultBase<List<T>>> GetAllAsync();
        Task<ResultBase<T?>> GetByIdAsync(string id);
        Task<ResultBase<T>> AddAsync(T entity);
        Task<ResultBase> UpdateAsync(T entity);
        Task<ResultBase> DeleteAsync(string id);
    }
}
