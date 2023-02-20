using Main.ViewModel;
using System;

namespace Main.Stores
{
    /// <summary>
    /// Storing the state of navigation so that it can be passed between viewmodles
    /// </summary>
    public class NavigationStore
    {

        public event Action CurrentViewModelChanged;

        private BaceViewModel _currentViewModel;
        public BaceViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewMOdelChanged();
            }

        }

        private void OnCurrentViewMOdelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
