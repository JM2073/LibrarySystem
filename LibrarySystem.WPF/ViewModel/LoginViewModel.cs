using System.Windows.Input;
using LibrarySystem.Service;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
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