using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class User
    {
        public int LibraryCardNumber { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfBooksCheckedOut { get; set; }
        public List<Book> Books { get; set; }
        public string Password { get; set; }
    }
}
