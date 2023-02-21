using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class LogoutCommand : CommandBace
    {
        private readonly AccountStore _userStore;
        private readonly INavigationService _navigationService;


        public LogoutCommand(AccountStore userStore, INavigationService navigationService)
        {
            _userStore = userStore;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _userStore.LogOut();
            _navigationService.Navigate();
        }
    }
}
