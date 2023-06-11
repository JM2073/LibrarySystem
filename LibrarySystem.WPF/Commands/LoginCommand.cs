using System;
using System.Windows;
using LibrarySystem.Service;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Commands
{
    public class LoginCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;
        private readonly LoginViewModel _viewModel;

        public LoginCommand(LoginViewModel viewModel, AccountStore accountStore, INavigationService navigationService)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        private AccountService AccountService => new AccountService();

        public override void Execute(object parameter)
        {
            //TODO: pass either the library card number or email to get the user.
            Guid.TryParse(_viewModel.Id, out var lcn);
            
            _accountStore.CurrentUser = AccountService.GetUser(lcn, _viewModel.Id);

            if (_accountStore.CurrentUser == null)
            {
                MessageBox.Show("There was an issue logging in.\nplease check your details and then try again.");
                return;
            }

            MessageBox.Show($"logging in {_accountStore.CurrentUser.Name}....");


            _navigationService.Navigate();
        }
    }
}