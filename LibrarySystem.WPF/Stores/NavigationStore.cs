using System;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Stores
{
    /// <summary>
    ///     Storing the state of navigation so that it can be passed between viewmodles
    /// </summary>
    public class NavigationStore
    {
        private BaceViewModel _currentViewModel;

        public BaceViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewMOdelChanged();
            }
        }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewMOdelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}