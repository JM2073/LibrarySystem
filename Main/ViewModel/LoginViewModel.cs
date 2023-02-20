using Main.Commands;
using Main.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModel
{
    public class LoginViewModel : BaceViewModel
    {
        /// <summary>
        /// command for changing the view to the ResgierView
        /// </summary>
        public ICommand NavigateRegisterCommand { get; }
        public ICommand LoginCommand { get; }
        public string Email { get; set; }
        public string Password { private get; set; }

        public LoginViewModel(UserStore userStore,NavigationStore navigationStore)
        {
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(new Servies.NavigationService<RegisterViewModel>(navigationStore, () =>  new RegisterViewModel(navigationStore)));

            LoginCommand = new LoginCommand(this, userStore,new Servies.NavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(userStore, navigationStore)));
        }
    }
}
