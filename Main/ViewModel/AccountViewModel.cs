using System;
using System.Data.SqlClient;
using Main.Stores;

namespace Main.ViewModel
{
    public class AccountViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;

        private readonly BookService _bookService;

        

        public string Name => _accountStore.CurrentUser?.Name;
        public string Email => _accountStore.CurrentUser?.Email;

        public AccountViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _bookService = new BookService();

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

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
