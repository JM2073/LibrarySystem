using System;
using LibrarySystem.Domain.Models;
using LibrarySystem.Service;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Commands
{
    public class AddUserCommand : CommandBace
    {
        
        private readonly AccountStore _accountStore;
        private readonly AddUserViewModel _vm;

        private readonly AccountService _accountService;
        public AddUserCommand(AccountStore accountStore, AddUserViewModel vm)
        {
            _accountStore = accountStore;
            _vm = vm;

            _accountService = new AccountService();
        }
        
        public override void Execute(object parameter)
        {
            AccountType at;
            switch (_vm.SelectedAccountType)
            {
                case "Librarian":
                    at = AccountType.Librarian;
                    break;
                case "Member":
                    at = AccountType.Member;
                    break;
                default:
                    throw new NotImplementedException("Type of user has not been accounted for.");
            }
            
            _accountService.AddUser(new User
                
            {
                LibraryCardNumber = Guid.NewGuid(),
                Name = $"{_vm.FirstName} {_vm.LastName}",
                PhoneNumber = _vm.PhoneNumber,
                Email = _vm.Email,
                AccountType = at
            });
            
        }
    }
}