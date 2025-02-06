using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.Application;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddApplicationServices();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTodos).Assembly));

            return services;
        }
    }
}
