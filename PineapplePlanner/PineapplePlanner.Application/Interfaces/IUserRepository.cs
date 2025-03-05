using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Interfaces
{
    public interface IUserRepository : IBaseRespository<Domain.Entities.User>
    {
        Task<ResultBase<Domain.Entities.User?>> GetByUIdAsync(string uid);
        Task<ResultBase<Domain.Entities.User>> CreateAsync(Domain.Entities.User user);
    }
}
