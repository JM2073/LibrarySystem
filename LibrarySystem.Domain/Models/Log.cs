using System;

namespace LibrarySystem.Domain.Models
{
    public class Log
    {
        public DateTime Date { get; set; }
        public string Isbn { get; set; }
        public string LibraryCardNumber { get; set; }
        public string Description { get; set; }
    }

}