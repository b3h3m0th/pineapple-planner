using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Application.Repositories;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRespository<IBaseFirestoreData>, BaseRepository<IBaseFirestoreData>>();

            return services;
        }
    }
}
