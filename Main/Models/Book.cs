using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int ISBN { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Summary { get; set; }
        public int AvailableCopies { get; set; }
        public string Genre { get; set; }
        public int Price { get; internal set; }
    }
}
