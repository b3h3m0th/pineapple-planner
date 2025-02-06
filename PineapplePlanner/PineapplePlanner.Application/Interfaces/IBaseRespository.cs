namespace PineapplePlanner.Application.Interfaces
{
    public interface IBaseRespository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<object> GetAsync(T entity);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<List<T>> QueryRecordsAsync(object query);
    }
}
