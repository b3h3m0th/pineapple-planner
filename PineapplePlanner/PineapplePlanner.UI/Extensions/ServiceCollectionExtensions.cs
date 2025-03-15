using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PineapplePlanner.AI.Extensions;
using PineapplePlanner.Application.Extensions;
using PineapplePlanner.UI.Providers;
using PineapplePlanner.UI.Services;

namespace PineapplePlanner.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddAIServices();
            services.AddScoped<AuthenticationStateProvider, FirebaseAuthStateProvider>();
            services.AddScoped<FirebaseAuthenticationService>();
            services.AddAuthorizationCore();
            services.AddSingleton<LocalizationService>();
            services.AddMudServices();

            return services;
        }
    }
}
