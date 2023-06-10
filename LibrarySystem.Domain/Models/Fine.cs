using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Models
{
    public class Fine
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [DataType(DataType.Currency)]
        public decimal FineAmount { get; set; }
        public string Reason { get; set; }
        public DateTime PayByDate { get; set; }
        [DefaultValue(false)]
        public bool IsArchived { get; set; }
        [DefaultValue(false)]
        public bool IsPayed { get; set; }
        [DefaultValue(false)]
        public bool FinalWarningSent { get; set; }
        public List<Log> logs { get; set; }


    }
}