using Main.Stores;

namespace Main.ViewModel
{
    public class MainViewModel : BaceViewModel
    {
        private readonly NavigationStore _navigationStore;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        /// <summary>
        ///     the view model that is currently loaded.
        /// </summary>
        public BaceViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        /// <summary>
        ///     notifies the view that the viewmodel has changed, causing changes to view
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChange(nameof(CurrentViewModel));
        }
    }
}