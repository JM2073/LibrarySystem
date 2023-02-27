using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchBarViewModel : BaceViewModel
    {
        public SearchBarViewModel(SearchStore searchStore, INavigationService searchNavigationService)
        {
            SearchCommand = new SearchCommand(this, searchStore, searchNavigationService);
        }

        public ICommand SearchCommand { get; }

        public string SearchString { get; set; }
    }
}