using System.Windows.Input;
using Main.Commands;
using Main.Servies;

namespace Main.ViewModel
{
    public class RegisterViewModel : BaceViewModel
    {
        public RegisterViewModel(INavigationService loginNavigationService)
        {
            RegisterCommand = new RegisterCommand(this, loginNavigationService);
        }

        public ICommand RegisterCommand { get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}