using System.Windows.Input;
using LibrarySystem.Service;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
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