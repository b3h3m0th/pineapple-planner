using Microsoft.Extensions.DependencyInjection;
using PineapplePlanner.UI.Extensions;
using System.Windows;

namespace PineapplePlanner.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow(serviceCollection);
            mainWindow.Show();
        }

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddWpfBlazorWebView();
            services.AddUIServices();
        }
    }
}
