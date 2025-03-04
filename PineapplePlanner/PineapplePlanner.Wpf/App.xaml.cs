using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.UI.Extensions;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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
                Title = "Pineapple Planner",
                Height = 720,
                Width = 1280,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Icon = new BitmapImage(
                    new Uri(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pineapple_planner_logo.png"),
                    UriKind.RelativeOrAbsolute))
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
