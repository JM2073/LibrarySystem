using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
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