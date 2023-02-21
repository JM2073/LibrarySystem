using Main.Servies;
using Main.ViewModel;

namespace Main.Commands
{
    public class NavigateCommand : CommandBace
        
    {
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
