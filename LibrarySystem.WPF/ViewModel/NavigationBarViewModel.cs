using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.Domain.Models;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class NavigationBarViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;

        public bool IsUserEditVisible { get; set; }
        public bool IsAllUsersVisible { get; set; }

        public string lbUserName { get; set; }

        public NavigationBarViewModel(AccountStore accountStore, INavigationService loginNavigationService,
            INavigationService accountNavigationService, INavigationService checkOutBooksNavigationService,
            INavigationService checkInBooksNavigationService, INavigationService editAccountDetailsService,
            INavigationService viewAllUsersService)
        {
            _accountStore = accountStore;
            IsAllUsersVisible = false;
            IsUserEditVisible = false;
            
            switch (accountStore.CurrentUser.AccountType)
            {
                case AccountType.Librarian:
                    IsAllUsersVisible = true;
                    IsUserEditVisible = true;
                    break;
                case AccountType.Member:
                    IsUserEditVisible = true;
                    break;
            }

            lbUserName = accountStore.CurrentUser.Name;

            NavigateEditAccountDetailsCommand = new NavigateCommand(editAccountDetailsService);
            NavigateViewAllUsersCommand = new NavigateCommand(viewAllUsersService);
            
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            NavigateCheckOutBookCommand = new NavigateCommand(checkOutBooksNavigationService);
            NavigateCheckInBookCommand = new NavigateCommand(checkInBooksNavigationService);
            LogoutCommand = new LogoutCommand(_accountStore, loginNavigationService);

            _accountStore.CurrentAccountChanged += OnCurrentAccountCHanged;
        }

        public ICommand NavigateViewAllUsersCommand { get; }

        public ICommand NavigateEditAccountDetailsCommand { get;}

        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateCheckInBookCommand { get; }
        public ICommand NavigateCheckOutBookCommand { get; }
        public ICommand LogoutCommand { get; }

        public bool IsLoggedIn => _accountStore.IsLoggedIn;

        private void OnCurrentAccountCHanged()
        {
            OnPropertyChange(nameof(IsLoggedIn));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountCHanged;

            base.Dispose();
        }
    }
}