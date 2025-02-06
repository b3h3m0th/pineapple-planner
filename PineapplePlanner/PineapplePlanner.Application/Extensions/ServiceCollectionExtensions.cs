using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Application.Repositories;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddInfrastructureServices();
            //services.AddScoped<IBaseRespository<IBaseFirestoreData>, BaseRepository<IBaseFirestoreData>>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}
