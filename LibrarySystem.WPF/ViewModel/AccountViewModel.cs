using System;
using System.Collections.ObjectModel;
using System.Windows;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using LibrarySystem.Service;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class AccountViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;
        
        private LibraryDBContextFactory _libraryDBContextFactory;
        private AccountService AccountService => new AccountService();
        private FineService FineService => new FineService();
        
        #region Collections

        // private readonly ObservableCollection<Book> _checkedOutBooks;
        // public IEnumerable<Book> CheckedOutBooks => _checkedOutBooks;  
        // private readonly ObservableCollection<Book> _dueBackBooks;
        // public IEnumerable<Book> DueBackBooks => _dueBackBooks;        
        // private readonly ObservableCollection<Fine> _outstandingFees;
        // public IEnumerable<Fine> OutstandingFees => _outstandingFees;

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
        private ObservableCollection<Book>  _dueBackBooks;
        public ObservableCollection<Book> DueBackBooks
        {
            get { return  _dueBackBooks; }
            set
            {
                _dueBackBooks = value;
                OnPropertyChange(nameof(DueBackBooks));
            }
        }
        private ObservableCollection<Fine>  _outstandingFees;
        private LibraryDBContextFactory _librrayDBContextFactory;

        public ObservableCollection<Fine> OutstandingFees
        {
            get { return  _outstandingFees; }
            set
            {
                _outstandingFees = value;
                OnPropertyChange(nameof(OutstandingFees));
            }
        }
        public void ReplaceCheckedOutBooksCollection()
        {
            CheckedOutBooks = AccountService.GetCheckedOutBooks(_accountStore.CurrentUser.Id);
        }       
        public void ReplaceDueBackBooksCollection()
        {
            DueBackBooks = AccountService.GetDueBackBooks(_accountStore.CurrentUser.Id);
        }
        public void ReplaceOutstandingFeesCollection()
        {
            OutstandingFees = AccountService.GetFines(_accountStore.CurrentUser.Id);
        }
        #endregion
        public AccountViewModel(AccountStore accountStore, LibraryDBContextFactory librrayDbContextFactory)
        {
            _accountStore = accountStore;
            _librrayDBContextFactory = librrayDbContextFactory;
            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
            
            ReplaceCheckedOutBooksCollection();
            ReplaceDueBackBooksCollection();
            ReplaceOutstandingFeesCollection();
        }

        private BookService BookService => new BookService(_accountStore.CurrentUser.LibraryCardNumber);

        public string Name => _accountStore.CurrentUser?.Name;
        public string Email => _accountStore.CurrentUser?.Email;

        private void OnCurrentAccountChanged()
        {
            OnPropertyChange(nameof(Name));
            OnPropertyChange(nameof(Email));
        }

        public void CheckInBook(string isbn)
        {
            try
            {
                BookService.CheckInBook(isbn, _accountStore.CurrentUser.LibraryCardNumber);
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

                BookService.RenewBook(isbn,_accountStore.CurrentUser.LibraryCardNumber);

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
        
        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
      
            base.Dispose();
        }
    }
}