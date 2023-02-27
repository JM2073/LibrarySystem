using Main.Servies;
using Main.Stores;

namespace Main.Commands
{
    public class LogoutCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly AccountStore _userStore;


        public LogoutCommand(AccountStore userStore, INavigationService navigationService)
        {
            _userStore = userStore;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _userStore.LogOut();
            _navigationService.Navigate();
        }
    }
}