using System;
using System.Windows;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;


namespace LibrarySystem.WPF
{
 public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            //singletons that are added to the services, these persist though out the applications lifetime
            services.AddSingleton<AccountStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<SearchStore>();

            services.AddSingleton(s=> CreateLoginNavigationService(s));
            
            //below sets up the view modules for all user-controls  
            services.AddTransient(CreateNavigationBarViewModel);
            services.AddTransient(s => new SearchBarViewModel(s.GetRequiredService<SearchStore>(), CreateSearchNavigationService(s)));
            services.AddTransient(s => new LoginViewModel(s.GetRequiredService<AccountStore>(),
                CreateAccountNavigationService(s), CreateRegisterNavigationService(s)));
            services.AddTransient(s => new RegisterViewModel(CreateLoginNavigationService(s), s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new AccountViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new SearchViewModel(s.GetRequiredService<SearchStore>(), s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new CheckInBookViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new CheckOutBookViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new AllUsersViewModel(s.GetRequiredService<AccountStore>(), CreateEditAccountDetailsNavigationService(s)));
            services.AddTransient(s => new AddUserViewModel(s.GetRequiredService<AccountStore>()));
            services.AddTransient(s => new EditUserViewModel(s.GetRequiredService<AccountStore>()));
            
            services.AddSingleton<MainViewModel>();
            services.AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            
            var initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            //checks if there are any fines in the xml files and added them or updates exsisting fines.
            var fineService = new FineService(new AccountStore());
            fineService.CheckFines();

            if (MessageBox.Show("GENERATE INITIAL LOGS?", "TEST", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                 var logService = new LogService();
                 logService.InitialLogs();
            }
            
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            //open the application for the user. 
            MainWindow.Show();
            base.OnStartup(e);
        }

        //within this region are the navigation service builders that are not within the layout 
        #region Navigation

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

        #endregion

        //within this region are the navigation service builders that are within the layout 
        #region LayoutNavigation

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            
            return new NavigationBarViewModel(
                serviceProvider.GetRequiredService<AccountStore>(),
                CreateLoginNavigationService(serviceProvider),
                CreateAccountNavigationService(serviceProvider),
                CreateCheckedOutBooksNavigationService(serviceProvider),
                CreateCheckedInBooksNavigationService(serviceProvider),
                CreateEditAccountDetailsNavigationService(serviceProvider),
                CreateViewAllUsersNavigationService(serviceProvider));
        }
        
        private INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AccountViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateSearchNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<SearchViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SearchViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }

        private INavigationService CreateCheckedOutBooksNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<CheckOutBookViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<CheckOutBookViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }
        private INavigationService CreateCheckedInBooksNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<CheckInBookViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<CheckInBookViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }
        private INavigationService CreateEditAccountDetailsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<EditUserViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<EditUserViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }
        private INavigationService CreateViewAllUsersNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AllUsersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AllUsersViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<SearchBarViewModel>());
        }
     #endregion
    }
}