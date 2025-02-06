using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.UI.Extensions
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
