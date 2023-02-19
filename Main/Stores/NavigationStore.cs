using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
