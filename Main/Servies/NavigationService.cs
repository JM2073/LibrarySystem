using System;
using Main.Stores;
using Main.ViewModel;

namespace Main.Servies
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