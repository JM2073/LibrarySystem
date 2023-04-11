using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class RegisterViewModel : BaceViewModel
    {
        public RegisterViewModel(INavigationService loginNavigationService, AccountStore accountStore)
        {
            RegisterCommand = new RegisterCommand(this, accountStore, loginNavigationService);
        }

        public ICommand RegisterCommand { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}