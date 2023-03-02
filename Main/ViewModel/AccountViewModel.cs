using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Main.Models;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class AccountViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;

        private readonly ObservableCollection<Book> _checkedOutBooks;
        public IEnumerable<Book> CheckedOutBooks => _checkedOutBooks;  
        
        private readonly ObservableCollection<Book> _dueBackBooks;
        public IEnumerable<Book> DueBackBooks => _dueBackBooks;        
        
        private readonly ObservableCollection<Fine> _outstandingFees;
        public IEnumerable<Fine> OutstandingFees => _outstandingFees;
        public AccountViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
            _checkedOutBooks = new ObservableCollection<Book>(_accountStore.CurrentUser.Books);
            _dueBackBooks = new ObservableCollection<Book>(_accountStore.CurrentUser.Books);
            _outstandingFees = new ObservableCollection<Fine>(_accountStore.CurrentUser.Fines);
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