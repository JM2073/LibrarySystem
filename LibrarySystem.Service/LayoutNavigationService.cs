namespace LibrarySystem.Service
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : BaceViewModel
    {
        private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
        private readonly Func<SearchBarViewModel> _createSearchBarViewModel;
        private readonly Func<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public LayoutNavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel,
            Func<NavigationBarViewModel> createNavigationBarViewModel,
            Func<SearchBarViewModel> createSearchBarViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _createNavigationBarViewModel = createNavigationBarViewModel;
            _createSearchBarViewModel = createSearchBarViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel(),
                _createSearchBarViewModel(), _createViewModel());
        }
    }
}