using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddSingleton<FirebaseService>();

            return services;
        }
    }
}
