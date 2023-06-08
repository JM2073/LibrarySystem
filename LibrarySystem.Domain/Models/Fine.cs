using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Models
{
    public class Fine
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public double FineAmount { get; set; }
        public string Reason { get; set; }
        public DateTime PayByDate { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        
        
        public List<Log> logs { get; set; }
    }
}