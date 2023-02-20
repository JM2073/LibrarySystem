using Main.Stores;
using Main.ViewModel;
using System.Windows;

namespace Main
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            UserStore userStore = new UserStore();

            //start the application with the Login view loaded by setting the LoginViewModel as the CurrentView 
            navigationStore.CurrentViewModel = new LoginViewModel(userStore, navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            MainWindow.Show();
            base.OnStartup(e);
        }

    }
}
