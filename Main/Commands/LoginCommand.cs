using System.Windows;
using Main.Models;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class LoginCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly AccountStore _userStore;
        private readonly LoginViewModel _viewModel;

        public LoginCommand(LoginViewModel viewModel, AccountStore userStore, INavigationService navigationService)
        {
            _viewModel = viewModel;
            _userStore = userStore;
            _navigationService = navigationService;
        }

        private AccountService _accountService => new AccountService();

        public override void Execute(object parameter)
        {
            User user;
            //TODO: pass either the library card number or email to get the user.
            user = _accountService.GetUser(null, _viewModel.Email);

            if (user == null)
            {
                MessageBox.Show("There was an issue logging in.\nplease check your details and then try again.");
                return;
            }

            _userStore.CurrentUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LibraryCardNumber = user.LibraryCardNumber,
                NumberOfBooksCheckedOut = user.NumberOfBooksCheckedOut,
                Books = user.Books
            };

            MessageBox.Show($"logging in {_userStore.CurrentUser.Name}....");


            _navigationService.Navigate();
        }
    }
}