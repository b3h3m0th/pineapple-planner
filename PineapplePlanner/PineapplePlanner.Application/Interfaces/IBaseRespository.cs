using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IBaseRespository<T> where T : IBaseFirestoreData
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
