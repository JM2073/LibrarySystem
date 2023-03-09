using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models
{
    public class User : INotifyPropertyChanged
    {
        public string LibraryCardNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfBooksCheckedOut => Books.Count;
        public AccountType AccountType { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Fine> Fines { get; set; }

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