using Main.Servies;
using Main.ViewModel;

namespace Main.Commands
{
    /// <summary>
    /// For navigating between views within the WPF MVVM application.
    /// This makes changing the view possable from within another view.
    /// </summary>
    /// <typeparam name="TViewModel"> takes a viewmodel that must inherit from BaceViewModel</typeparam>
    public class NavigateCommand<TViewModel> : CommandBace
        where TViewModel : BaceViewModel
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }




        /// <summary>
        /// sets the current viewmodel to the passed model.
        /// </summary>
        /// <param name="parameter">parameter that was passed with the usage.</param>
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
