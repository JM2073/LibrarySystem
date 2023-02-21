using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class NavigationBarViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateCheckInBookCommand { get; }
        public ICommand NavigateCheckOutBookCommand { get; }
        public ICommand LogoutCommand { get; }

        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public NavigationBarViewModel(AccountStore accountStore, INavigationService loginNavigationService,
            INavigationService accountNavigationService)
        {
            _accountStore = accountStore;

            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            LogoutCommand = new LogoutCommand(_accountStore,loginNavigationService);

            _accountStore.CurrentAccountChanged += OnCurrentAccountCHanged;
        }

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
