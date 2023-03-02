using System;
using Main.Models;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class RegisterCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly RegisterViewModel _viewModel;
        private readonly AccountStore _accountStore;

        public RegisterCommand(RegisterViewModel registerViewModel,AccountStore accountStore, INavigationService navigationService)
        {
            _viewModel = registerViewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        private AccountService _accountService => new AccountService(_accountStore);

        public override void Execute(object parameter)
        {
            
            _accountService.AddUser(new User
            {
                LibraryCardNumber = Guid.NewGuid().ToString(),
                Name = $"{_viewModel.FirstName} {_viewModel.LastName}",
                PhoneNumber = _viewModel.PhoneNumber,
                Email = _viewModel.Email,
                AccountType = AccountType.Member
            });

            //TODO work in validation.

            _navigationService.Navigate();
        }
    }
}