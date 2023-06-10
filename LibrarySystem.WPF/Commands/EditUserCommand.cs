using System;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.Commands
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
                _accountStore.EditUserId = (Guid)parameter;
            }

            _navigationService.Navigate();

            _accountStore.EditUserId = null;
        }
    }
}