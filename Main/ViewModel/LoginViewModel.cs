using Main.Commands;
using Main.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModel
{
    public class LoginViewModel : BaceViewModel
    {
        /// <summary>
        /// command for changing the view to the ResgierView
        /// </summary>
        public ICommand NavigateRegisterCommand { get; }

        public LoginViewModel(NavigationStore navigationStore)
        {
            //loading the command with the correct viewmodles.
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(navigationStore, () => new RegisterViewModel(navigationStore));
        }

    }
}
