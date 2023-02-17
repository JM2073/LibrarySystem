using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class User
    {
        public User(int libraryCardNumber, string name, int phoneNumber, string email, int numberOfBooksCheckedOut, List<Book> books)
        {
            LibraryCardNumber = libraryCardNumber;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            NumberOfBooksCheckedOut = numberOfBooksCheckedOut;
            Books = books;
        }

        public int LibraryCardNumber { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfBooksCheckedOut { get; set; }
        public List<Book> Books { get; set; }
        public string Password { get; set; }
    }
}
