using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.AI.Services;

namespace PineapplePlanner.AI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAIServices(this IServiceCollection services)
        {
            services.AddScoped<AIService>();

            return services;
        }
    }
}
