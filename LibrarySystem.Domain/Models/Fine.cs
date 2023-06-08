using System;

namespace LibrarySystem.Domain.Models
{
    public class Fine
    {
        public double FineAmount { get; set; }
        public string Reason { get; set; }
        
        public string BookTitle { get; set; }
        public DateTime PayByDate { get; set; }
        public string Isbn { get; set; }
    }
}