using Microsoft.Extensions.DependencyInjection;
using RentestWPFTestTask.Services;
using RentestWPFTestTask.ViewModels;
using RentestWPFTestTask.Views;
using System;
using System.Windows;

namespace RentestWPFTestTask
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IImageDialogService, ImageDialogService>();
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindowViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}