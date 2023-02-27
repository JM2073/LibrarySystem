using System;
using Main.Stores;
using Main.ViewModel;

namespace Main.Servies
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : BaceViewModel
    {
        private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
        private readonly Func<SearchBarViewModel> _CreateSearchBarViewModel;
        private readonly Func<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public LayoutNavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel,
            Func<NavigationBarViewModel> createNavigationBarViewModel,
            Func<SearchBarViewModel> createSearchBarViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _createNavigationBarViewModel = createNavigationBarViewModel;
            _CreateSearchBarViewModel = createSearchBarViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel(),
                _CreateSearchBarViewModel(), _createViewModel());
        }
    }
}