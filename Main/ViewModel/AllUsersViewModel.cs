using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Main.Commands;
using Main.Models;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
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
            
            dti = _accountService.GetAllUsers().Select(x=> new DataTableItem
            {
                LibraryCardNumber = x.LibraryCardNumber,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                AccountType = x.AccountType,
                TotalFees = _accountService.GetFines(x.LibraryCardNumber).Sum(z=>z.FineAmount).ToString("C2"),
                BooksCheckedOut = _accountService.GetCheckedOutBooks(x.LibraryCardNumber).Count(),
                BooksOverDue = _accountService.GetDueBackBooks(x.LibraryCardNumber).Count()
                
            }).ToList();
        }

        public ICommand EditUserCommand;

        public void EditUser(string libraryCardNumber)
        {
            EditUserCommand.Execute(libraryCardNumber);
        }
    }
}