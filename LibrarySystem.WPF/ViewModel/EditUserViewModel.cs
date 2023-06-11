using System;
using System.Collections.ObjectModel;
using System.Windows;
using LibrarySystem.Domain.Models;
using LibrarySystem.Service;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class EditUserViewModel : BaceViewModel
    {
        private int Id { get; set; }
        public Guid LibraryCardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLibrarian { get; set; }

        private readonly AccountStore _accountStore;
        public ObservableCollection<string> AccountTypes { get; set; }
        public string SelectedAccountType { get; set; }
        private FineService FineService => new FineService();
        private AccountService _accountService { get; set; }


        public EditUserViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
            IsLibrarian = _accountStore.CurrentUser.AccountType == AccountType.Librarian;

            _accountService = new AccountService(); 
            LibraryCardNumber = _accountStore.EditUserId ?? _accountStore.CurrentUser.LibraryCardNumber;

            var user = _accountService.GetUser(LibraryCardNumber, null);


            //Populate view
            int lastSpaceIndex = user.Name.LastIndexOf(' ');
            Id = user.Id;
            FirstName = user.Name.Substring(0, lastSpaceIndex);
            LastName = user.Name.Substring(lastSpaceIndex + 1);
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            AccountTypes = new ObservableCollection<string>
            {
                "Librarian",
                "Member"
            };

            switch (user.AccountType)
            {
                case AccountType.Librarian:
                    SelectedAccountType = "Librarian";
                    break;
                case AccountType.Member:
                    SelectedAccountType = "Member";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            ReplaceCheckedOutBooksCollection();
            ReplaceDueBackBooksCollection();
            ReplaceOutstandingFeesCollection();
        }


        #region Collections

        private ObservableCollection<Book> _checkedOutBooks;

        public ObservableCollection<Book> CheckedOutBooks
        {
            get { return _checkedOutBooks; }
            set
            {
                _checkedOutBooks = value;
                OnPropertyChange(nameof(CheckedOutBooks));
            }
        }

        private ObservableCollection<Book> _dueBackBooks;

        public ObservableCollection<Book> DueBackBooks
        {
            get { return _dueBackBooks; }
            set
            {
                _dueBackBooks = value;
                OnPropertyChange(nameof(DueBackBooks));
            }
        }

        private ObservableCollection<Fine> _outstandingFees;

        public ObservableCollection<Fine> OutstandingFees
        {
            get { return _outstandingFees; }
            set
            {
                _outstandingFees = value;
                OnPropertyChange(nameof(OutstandingFees));
            }
        }

        public void ReplaceCheckedOutBooksCollection()
        {
            CheckedOutBooks = _accountService.GetCheckedOutBooks(Id);
        }

        public void ReplaceDueBackBooksCollection()
        {
            DueBackBooks = _accountService.GetDueBackBooks(Id);
        }

        public void ReplaceOutstandingFeesCollection()
        {
            OutstandingFees = _accountService.GetFines(Id);
        }

        #endregion

        private BookService BookService => new BookService(_accountStore.CurrentUser.LibraryCardNumber);


        public void CheckInBook(string isbn)
        {
            try
            {
                BookService.CheckInBook(isbn, LibraryCardNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
                return;
            }

            ReplaceCheckedOutBooksCollection();
            ReplaceDueBackBooksCollection();
        }

        public void PayFine(int id)
        {
            FineService.PayFine(id);
            ReplaceOutstandingFeesCollection();
        }

        public void RenewBook(string isbn)
        {
            try
            {
                BookService.RenewBook(isbn, LibraryCardNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
                return;
            }

            ReplaceCheckedOutBooksCollection();
            ReplaceDueBackBooksCollection();
        }
    }
}