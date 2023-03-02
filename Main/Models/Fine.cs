using System;

namespace Main.Models
{
    public class Fine
    {
        public double FineAmount { get; set; }
        public string Reason { get; set; }
        public DateTime PayByDate { get; set; }
        public string ISBN { get; set; }
    }
}