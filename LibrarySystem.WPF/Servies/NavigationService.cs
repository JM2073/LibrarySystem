using System;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Servies
{
    public class NavigationService<TViewModel> : INavigationService where TViewModel : BaceViewModel
    {
        private readonly Func<TViewModel> _createVewModel;
        private readonly NavigationStore _navigationStore;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createVewModel)
        {
            _navigationStore = navigationStore;
            _createVewModel = createVewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createVewModel();
        }
    }
}