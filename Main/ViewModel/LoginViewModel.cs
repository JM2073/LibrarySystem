using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class LoginViewModel : BaceViewModel
    {
        public LoginViewModel(AccountStore userStore, INavigationService accountNavigationService,
            INavigationService registerNavigationService)
        {
            NavigateRegisterCommand = new NavigateCommand(registerNavigationService);

            LoginCommand = new LoginCommand(this, userStore, accountNavigationService);
        }

        public ICommand NavigateRegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public string Id { get; set; }
    }
}