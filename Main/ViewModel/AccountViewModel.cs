using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Main.Models;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class AccountViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;

        private AccountService _AccountService => new AccountService(_accountStore);
        
        #region Collections

        private readonly ObservableCollection<Book> _checkedOutBooks;
        public IEnumerable<Book> CheckedOutBooks => _checkedOutBooks;  
        
        private readonly ObservableCollection<Book> _dueBackBooks;
        public IEnumerable<Book> DueBackBooks => _dueBackBooks;        
        
        private readonly ObservableCollection<Fine> _outstandingFees;
        public IEnumerable<Fine> OutstandingFees => _outstandingFees;

        #endregion
        public AccountViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;

            _checkedOutBooks = _AccountService.GetCheckedOutBooks();
            _dueBackBooks = _AccountService.GetDueBackBooks();
            _outstandingFees = _AccountService.GetFines();
        }

        private BookService _bookService => new BookService(_accountStore);

        public string Name => _accountStore.CurrentUser?.Name;
        public string Email => _accountStore.CurrentUser?.Email;

        private void OnCurrentAccountChanged()
        {
            OnPropertyChange(nameof(Name));
            OnPropertyChange(nameof(Email));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;

            base.Dispose();
        }
    }
}