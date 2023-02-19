using Main.Stores;

namespace Main.ViewModel
{
    public class MainViewModel : BaceViewModel
    {
        private readonly NavigationStore _navigationStore;

        /// <summary>
        /// the view model that is currently loaded.
        /// </summary>
        public BaceViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChange(nameof(CurrentViewModel));
        }
    }
}
