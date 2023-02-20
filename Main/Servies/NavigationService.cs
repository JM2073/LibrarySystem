using Main.Stores;
using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Servies
{
    public class NavigationService<TViewModel>
           where TViewModel : BaceViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createVewModel;

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
