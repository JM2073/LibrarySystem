using System;
using System.Windows;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<AccountStore>();
            services.AddSingleton<NavigationStore>();

            services.AddSingleton<INavigationService>(s => CreateLoginNavigationService(s));

            services.AddTransient<NavigationBarViewModel>(s => CreateNavigationBarViewModel(s));
            services.AddTransient<SearchBarViewModel>(s => new SearchBarViewModel(CreateSearchNavigationService(s)));
            services.AddTransient<LoginViewModel>(s => new LoginViewModel(s.GetRequiredService<AccountStore>(),CreateAccountNavigationService(s),CreateRegisterNavigationService(s)));
            services.AddTransient<RegisterViewModel>(s => new RegisterViewModel(s.GetRequiredService<NavigationStore>()));
            services.AddTransient<AccountViewModel>(s => new AccountViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient<SearchViewModel>(s => new SearchViewModel());
            services.AddTransient<CheckInBookViewModel>();
            services.AddTransient<CheckOutBookViewModel>();

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            var initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();


            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            MainWindow.Show();
            base.OnStartup(e);
        }

        private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LoginViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LoginViewModel>());
        }

        private INavigationService CreateRegisterNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<RegisterViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<RegisterViewModel>());
        }

        private INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AccountViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                ()=>serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateSearchNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<SearchViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SearchViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                serviceProvider.GetRequiredService<AccountStore>(),
                CreateLoginNavigationService(serviceProvider), 
                CreateAccountNavigationService(serviceProvider));
        }

    }
}