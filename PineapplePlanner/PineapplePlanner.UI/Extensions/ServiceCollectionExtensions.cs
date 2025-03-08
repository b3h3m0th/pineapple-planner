using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PineapplePlanner.AI.Extensions;
using PineapplePlanner.Application;
using PineapplePlanner.UI.Localization;
using PineapplePlanner.UI.Providers;
using PineapplePlanner.UI.Services;
using System.Globalization;

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

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.AddScoped<IStringLocalizer<Resource>, StringLocalizer<Resource>>();

            string[] supportedCultures = [Culture.English, Culture.Swedish];

            CultureInfo culture = new(supportedCultures[1]);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture(Culture.Swedish);
            //    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
            //    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
            //    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            //});

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTodos).Assembly));

            services.AddMudServices();

            return services;
        }
    }
}
