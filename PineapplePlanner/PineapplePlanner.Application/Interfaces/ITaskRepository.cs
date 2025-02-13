namespace PineapplePlanner.Application.Interfaces
{
    public interface ITaskRepository : IBaseRespository<Domain.Entities.Task>
    {
        //Task<ResultBase<List<Domain.Entities.Task>>> GetAllAsync(string userUid);
        //Task<ResultBase<Domain.Entities.Task>> GetByIdAsync(string id, string userUid);
        //Task<ResultBase> AddAsync(Domain.Entities.Task entity, string userUid);
    }
}
