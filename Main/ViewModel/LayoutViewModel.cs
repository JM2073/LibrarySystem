using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModel
{
    public class LayoutViewModel : BaceViewModel
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public BaceViewModel ContentViewModel { get; }
        public SearchBarViewModel SearchBarViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, SearchBarViewModel searchBarViewModel, BaceViewModel contentViewModel)
        {
            ContentViewModel = contentViewModel;
            SearchBarViewModel = searchBarViewModel;
            NavigationBarViewModel = navigationBarViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            SearchBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
