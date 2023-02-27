using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class SearchCommand : CommandBace
    {
        private readonly INavigationService _navigationService;
        private readonly SearchStore _searchStore;
        private readonly SearchBarViewModel _viewModel;

        public SearchCommand(SearchBarViewModel searchBarViewModel, SearchStore searchStore,
            INavigationService navigationService)
        {
            _viewModel = searchBarViewModel;
            _searchStore = searchStore;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _searchStore.SearchString = _viewModel.SearchString;
            _navigationService.Navigate();
        }
    }
}