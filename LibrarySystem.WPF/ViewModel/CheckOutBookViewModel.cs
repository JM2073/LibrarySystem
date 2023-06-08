using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
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