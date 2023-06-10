using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LibrarySystem.Domain.Models;

    public class Book 
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal BookCost { get; set; }
        public DateTime? CheckedOutDate { get; set; }
        public DateTime? DueBackDate { get; set; }
        public bool HasBeenRenewed { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        public List<Log> Logs { get; set; }
        public List<Fine> Fines { get; set; }
        [DefaultValue(false)]
        public bool isArcived { get; set; }
        
        [NotMapped]
        public int AvailabilityCount { get; set; }
        [NotMapped]
        public bool IsCheckedOut => this.CheckedOutDate != null;
    }
