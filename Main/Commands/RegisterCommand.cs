using Main.Servies;
using Main.ViewModel;

namespace Main.Commands
{
    public class RegisterCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly RegisterViewModel _viewModel;

        public RegisterCommand(RegisterViewModel registerViewModel, INavigationService navigationService)
        {
            _viewModel = registerViewModel;
            _navigationService = navigationService;
        }

        private AccountService _accountService => new AccountService();

        public override void Execute(object parameter)
        {
            _accountService.AddUser($"{_viewModel.FirstName} {_viewModel.LastName}", _viewModel.Email,
                _viewModel.PhoneNumber);

            //TODO work in validation.
            // if (sucess)
            // {
            //     MessageBox.Show("registration Successful\nplease login with your details");
            //     _navigationService.Navigate();
            // }
            // else
            // {
            //     MessageBox.Show("There seems to be an issue.\nPlease try again.");
            // }
            _navigationService.Navigate();
        }
    }
}