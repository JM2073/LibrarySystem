using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Models
{
    public class Log 
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public int? FineId { get; set; }
        
        public DateTime Date { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        [ForeignKey("FineId")]
        public Fine? Fine { get; set; }
    }

}