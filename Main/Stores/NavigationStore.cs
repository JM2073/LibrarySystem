using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Stores
{
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
