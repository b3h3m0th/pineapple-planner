using Microsoft.Extensions.DependencyInjection;

namespace PineapplePlanner.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<FirestoreService>();

            return services;
        }
    }
}
