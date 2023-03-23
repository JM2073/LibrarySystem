using System.Windows.Input;
using Main.Commands;
using Main.Stores;

namespace Main.ViewModel
{
    public class CheckOutBookViewModel : BaceViewModel
    {
        public string BookIsbn { get; set; }

        public CheckOutBookViewModel(AccountStore accountStore)
        {
            CheckOutCommand = new CheckOutCommand(accountStore ,this);
        }
        public ICommand CheckOutCommand { get; set; }
    }
}