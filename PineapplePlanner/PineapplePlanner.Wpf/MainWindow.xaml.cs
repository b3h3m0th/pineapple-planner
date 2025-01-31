using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace PineapplePlanner.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IServiceCollection serviceCollection)
        {
            InitializeComponent();

            blazorWebView.Services = serviceCollection.BuildServiceProvider();
        }
    }
}
