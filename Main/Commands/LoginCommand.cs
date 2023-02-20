using Main.Models;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;
using System.Windows;

namespace Main.Commands
{
    public class LoginCommand : CommandBace
    {
        private readonly LoginViewModel _viewModel;
        private readonly NavigationService<AccountViewModel> _navigationService;
        private readonly UserStore _userStore;

        public LoginCommand(LoginViewModel viewModel, UserStore userStore, NavigationService<AccountViewModel> navigationService)
        {
            _viewModel = viewModel;
            _userStore = userStore;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            MessageBox.Show($"loggin in {_viewModel.Email}....");

            _userStore.CurrentUser = new User()
            {
                Name = "Testinson",
                Email = _viewModel.Email,
                PhoneNumber = 0,
                LibraryCardNumber = 0

            };


            _navigationService.Navigate();
        }
    }
}
