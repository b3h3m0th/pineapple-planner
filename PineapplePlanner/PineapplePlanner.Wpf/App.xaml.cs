﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.UI.Extensions;
using System.IO;
using System.Windows;

namespace PineapplePlanner.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureConfiguration();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow(serviceCollection)
            {
                Height = 720,
                Width = 1280,
                Title = "Pineapple Planner"
            };
            mainWindow.Show();
        }

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddWpfBlazorWebView();

            if (Configuration != null)
            {
                services.AddSingleton(Configuration);
                services.AddUIServices(Configuration);
            }
        }

        private void ConfigureConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }
}
