using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Main.Commands;
using Main.Servies;

namespace Main.ViewModel
{
    public class SearchBarViewModel : BaceViewModel
    {
        public ICommand SearchCommand { get; }

        public SearchBarViewModel(INavigationService searchNavigationService)
        {
            SearchCommand = new NavigateCommand(searchNavigationService);

        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
