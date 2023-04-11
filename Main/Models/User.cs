using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _libraryCardNumber;
        private string _name;
        private string _phoneNumber;
        private string _email;
        private AccountType _accountType;

        public string LibraryCardNumber
        {
            get => _libraryCardNumber;
            set
            {
                if (value == _libraryCardNumber) return;
                _libraryCardNumber = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value == _phoneNumber) return;
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        public AccountType AccountType
        {
            get => _accountType;
            set
            {
                if (value == _accountType) return;
                _accountType = value;
                OnPropertyChanged();
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public enum AccountType
    {
        Librarian,
        Member
        
    }
    
}