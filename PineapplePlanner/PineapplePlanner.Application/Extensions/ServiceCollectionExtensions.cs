using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Application.Repositories;
using PineapplePlanner.Application.Services;
using PineapplePlanner.Infrastructure.Extensions;

namespace PineapplePlanner.Application.Extensions
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
