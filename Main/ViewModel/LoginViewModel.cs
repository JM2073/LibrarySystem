using Main.Commands;
using Main.Stores;
using System.Windows.Input;
using Main.Servies;

namespace Main.ViewModel
{
    public class LoginViewModel : BaceViewModel
    {
        public ICommand NavigateRegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public string Email { get; set; }
        public string Password { private get; set; }

        public LoginViewModel(AccountStore userStore, INavigationService accountNavigationService,
            INavigationService registerNavigationService)
        {
            NavigateRegisterCommand = new NavigateCommand(registerNavigationService);
          
            LoginCommand = new LoginCommand(this, userStore, accountNavigationService);
        }
    }
}
