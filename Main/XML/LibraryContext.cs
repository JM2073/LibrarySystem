using Main.Models;

namespace Main.XML
{
    public class LibraryContext : XmlContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }

        public LibraryContext(string dataFolder)
            : base(dataFolder)
        {
        }
    }
}