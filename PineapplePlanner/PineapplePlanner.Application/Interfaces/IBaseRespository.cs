using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IBaseRespository<T> where T : IBaseFirestoreData
    {
        Task<List<T>> GetAllAsync();
        Task<object> GetAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> QueryRecordsAsync(object query);
    }
}
