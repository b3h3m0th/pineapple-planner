using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IEntryRepository : IBaseRespository<Domain.Entities.Entry>
    {
        Task<ResultBase<List<T>>> GetAllAsync<T>(string userId) where T : Domain.Entities.Entry;
        new Task<ResultBase<Domain.Entities.Entry>> AddAsync(Domain.Entities.Entry entry);
        new Task<ResultBase<Domain.Entities.Entry>> UpdateAsync(Domain.Entities.Entry entry);
        Task<ResultBase<List<Domain.Entities.Entry>>> DeleteAllAsync(string userId);
    }
}
