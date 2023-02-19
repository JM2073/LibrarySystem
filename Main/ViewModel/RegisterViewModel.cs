using Main.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModel
{
    public class RegisterViewModel : BaceViewModel
    {
        private NavigationStore navigationStore;

        public RegisterViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }
    }
}
