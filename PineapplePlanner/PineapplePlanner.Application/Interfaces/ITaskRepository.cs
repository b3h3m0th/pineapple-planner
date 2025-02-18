using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Interfaces
{
    public interface ITaskRepository : IBaseRespository<Domain.Entities.Task>
    {
        new Task<ResultBase<Domain.Entities.Task>> AddAsync(Domain.Entities.Task task);
        new Task<ResultBase<Domain.Entities.Task>> UpdateAsync(Domain.Entities.Task task);
    }
}
