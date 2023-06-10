using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LibrarySystem.Domain.Models
{
    public class User 
    {
        [Key]
        public int Id { get; set; }
        public Guid LibraryCardNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AccountType AccountType { get; set; }
        
        public List<Book> Books { get; set; }
        public List<Log> Logs { get; set; } 
        public List<Fine> Fines { get; set; }
        [DefaultValue(false)]
        public bool isArcived { get; set; }
    }
    public enum AccountType
    {
        Librarian,
        Member
        
    }
    
}