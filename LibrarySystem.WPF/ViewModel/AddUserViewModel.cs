using System.Collections.ObjectModel;
using System.Windows.Input;
using LibrarySystem.WPF.Commands;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.ViewModel
{
    public class AddUserViewModel : BaceViewModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public ObservableCollection<string> AccountTypes { get; set; }
        
        public string SelectedAccountType { get; set; }

        public AddUserViewModel(AccountStore accountStore)
        {
            AccountTypes = new ObservableCollection<string>
            {
                "Librarian",
                "Member"
            };

            AddUserCommand = new AddUserCommand(accountStore, this);
            
        }
        
        public ICommand AddUserCommand { get; }

    }
}