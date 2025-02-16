using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PineapplePlanner.Application;
using PineapplePlanner.UI.Providers;
using PineapplePlanner.UI.Services;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddApplicationServices();

            services.AddScoped<AuthenticationStateProvider, FirebaseAuthStateProvider>();
            services.AddScoped<FirebaseAuthenticationService>();
            services.AddAuthorizationCore();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTodos).Assembly));

            services.AddMudServices();

            return services;
        }
    }
}
