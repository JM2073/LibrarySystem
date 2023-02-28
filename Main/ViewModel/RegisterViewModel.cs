using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class RegisterViewModel : BaceViewModel
    {
        private readonly AccountStore _accountStore;

        public RegisterViewModel(INavigationService loginNavigationService, AccountStore accountStore)
        {
            _accountStore = accountStore;
            RegisterCommand = new RegisterCommand(this, _accountStore, loginNavigationService);
        }

        public ICommand RegisterCommand { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}