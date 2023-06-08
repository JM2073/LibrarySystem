using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class CheckInBookViewModel : BaceViewModel
    {
        public string BookIsbn { get; set; }
        
        public CheckInBookViewModel(AccountStore accountStore)
        {
            CheckInCommand = new CheckInCommand(accountStore, this);
        }
        public ICommand CheckInCommand { get; set; }
    }
}