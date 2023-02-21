using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Stores;
using Main.ViewModel;

namespace Main.Servies
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : BaceViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;
        private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
        private readonly Func<SearchBarViewModel> _CreateSearchBarViewModel;

        public LayoutNavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel, Func<NavigationBarViewModel> createNavigationBarViewModel, Func<SearchBarViewModel> createSearchBarViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _createNavigationBarViewModel = createNavigationBarViewModel;
            _CreateSearchBarViewModel = createSearchBarViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel(),_CreateSearchBarViewModel(),_createViewModel());
        }
    }
}
