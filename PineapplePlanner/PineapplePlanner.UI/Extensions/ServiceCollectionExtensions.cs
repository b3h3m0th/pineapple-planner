using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PineapplePlanner.AI.Extensions;
using PineapplePlanner.Application;
using PineapplePlanner.UI.Providers;
using PineapplePlanner.UI.Services;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();
            services.AddAIServices(configuration);
            services.AddScoped<AuthenticationStateProvider, FirebaseAuthStateProvider>();
            services.AddScoped<FirebaseAuthenticationService>();
            services.AddAuthorizationCore();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTodos).Assembly));

            services.AddMudServices();

            return services;
        }
    }
}
