using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Application.Repositories;
using PineapplePlanner.Application.Services;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddInfrastructureServices();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<SecureStorageService>();

            return services;
        }
    }
}
