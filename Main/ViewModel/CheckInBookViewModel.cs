using System.Windows.Input;
using Main.Commands;
using Main.Stores;

namespace Main.ViewModel
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