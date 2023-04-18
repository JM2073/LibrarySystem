using System.Windows;
using Main.Servies;
using Main.Stores;

namespace Main.Commands
{
    public class EditUserCommand: CommandBace
    {
        private AccountStore _accountStore;
        private readonly INavigationService _navigationService;

        public EditUserCommand(AccountStore accountStore, INavigationService navigationService)
        {
            _accountStore = accountStore;
            _navigationService = navigationService;
        }
        
        public override void Execute(object parameter)
        {
            if (parameter != null)
            {
                _accountStore.EditUserId = (string)parameter;
            }

            _navigationService.Navigate();

            _accountStore.EditUserId = null;
        }
    }
}