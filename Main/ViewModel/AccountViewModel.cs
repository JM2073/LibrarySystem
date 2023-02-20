using Main.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModel
{
    public class AccountViewModel : BaceViewModel
    {
        private readonly UserStore _userStore;

        public string Name => _userStore.CurrentUser?.Name;
        public string Email => _userStore.CurrentUser?.Email;

        public AccountViewModel(UserStore userStore, NavigationStore navigationStore)
        {
            _userStore = userStore; 
        }
    }
}
