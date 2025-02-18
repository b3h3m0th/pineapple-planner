using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IBaseRespository<T> where T : IBaseFirestoreData
    {
        Task<ResultBase<List<T>>> GetAllAsync(string userId);
        Task<ResultBase<T?>> GetByIdAsync(string userId, string id);
        Task<ResultBase<T>> AddAsync(T entity);
        Task<ResultBase<T>> UpdateAsync(T entity);
        Task<ResultBase> DeleteAsync(string id);
    }
}
