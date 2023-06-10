using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.Domain.Models;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class DataTableItem : User
    {
        public int BooksCheckedOut { get; set; }
        public int BooksOverDue { get; set; }
        public string TotalFees { get; set; }
    }
    
    public class AllUsersViewModel : BaceViewModel
    {
        private AccountStore _accountStore;

        public List<DataTableItem> dti { get; set; }


        private AccountService _accountService;
        public AllUsersViewModel(AccountStore accountStore, INavigationService editUserNavigationService)
        {
            _accountStore = accountStore;
            EditUserCommand = new EditUserCommand(_accountStore,editUserNavigationService);

            _accountService = new AccountService(_accountStore);
            
            dti = _accountService.GetAllActiveUsers().Select(x=> new DataTableItem
            {
                LibraryCardNumber = x.LibraryCardNumber,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                AccountType = x.AccountType,
                TotalFees = _accountService.GetFines(x.Id).Sum(z=>z.FineAmount).ToString("C2"),
                BooksCheckedOut = _accountService.GetCheckedOutBooks(x.Id).Count(),
                BooksOverDue = _accountService.GetDueBackBooks(x.Id).Count()
                
            }).ToList();
        }

        public ICommand EditUserCommand;

        public void EditUser(Guid libraryCardNumber)
        {
            EditUserCommand.Execute(libraryCardNumber);
        }
    }
}