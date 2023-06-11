using System;
using LibrarySystem.Domain.Models;
using LibrarySystem.Service;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Commands
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

        private AccountService AccountService => new AccountService();

        public override void Execute(object parameter)
        {
            
            AccountService.AddUser(new User
            {
                LibraryCardNumber = Guid.NewGuid(),
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