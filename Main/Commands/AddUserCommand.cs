using System;
using Main.Models;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
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

            _accountService = new AccountService(accountStore);
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
                LibraryCardNumber = Guid.NewGuid().ToString().Remove(8),
                Name = $"{_vm.FirstName} {_vm.LastName}",
                PhoneNumber = _vm.PhoneNumber,
                Email = _vm.Email,
                AccountType = at
            });
            
        }
    }
}