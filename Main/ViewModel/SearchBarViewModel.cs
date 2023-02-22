using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchBarViewModel : BaceViewModel
    {
        public ICommand SearchCommand { get; }
        
        public string SearchString { get; set; }
        
        public SearchBarViewModel(SearchStore searchStore,INavigationService searchNavigationService)
        {
            
            SearchCommand = new SearchCommand(this,searchStore,searchNavigationService);
        }
    }
}
