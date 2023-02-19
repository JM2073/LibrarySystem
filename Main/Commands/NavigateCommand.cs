using Main.Stores;
using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createVewModel;

        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createVewModel)
        {
            _navigationStore = navigationStore;
            _createVewModel = createVewModel;
        }

        /// <summary>
        /// sets the current viewmodel to the passed model.
        /// </summary>
        /// <param name="parameter">parameter that was passed with the usage.</param>
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = _createVewModel();
        }
    }
}
