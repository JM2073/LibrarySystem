using System.Xml.Linq;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.UnitTests;

public class Tests
{
    private static DbContextOptions<LibraryDbContext> dbContextOptions =
        new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase("Data").Options;

    private LibraryDbContext _context;
        
    [OneTimeSetUp]
    public void Setup()
    {
        _context = new LibraryDbContext(dbContextOptions);
        _context.Database.EnsureCreated();

        SeedDatabase();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    } 
    
    
    private void SeedDatabase()
    {
        _context.Users.AddRange(new List<User>
        {
            new User
            {
                Id = 1,
                LibraryCardNumber = Guid.NewGuid(),
                Name = "Johnathon Millard",
                PhoneNumber = "0123654987",
                Email = "JM@email.com",
                AccountType = AccountType.Librarian,
            },
            new User
            {
                Id = 2,
                LibraryCardNumber = Guid.NewGuid(),
                Name = "Emily Hancox",
                PhoneNumber = "0123654987",
                Email = "EH@email.com",
                AccountType = AccountType.Member,
            }
        });

        List<Book> books = new List<Book>();
        for (int i = 0; i < 5; i++)
        {
            books.AddRange(new List<Book>
            {
                new Book
                {
                    Id = 0,
                    Title = "A Game of Thrones",
                    Author = "George R. R. Martin",
                    Isbn = "9780553103540",
                    Publisher = "Bantam Spectra",
                    PublishDate = DateTime.Now,
                    Description = "The first book in the epic fantasy series A Song of Ice and Fire, which follows the struggles of several noble houses as they fight for control of the Seven Kingdoms.",
                    Genre = "Fantasy",
                    BookCost = 30.00m,
                    CheckedOutDate = null,
                    DueBackDate = null,
                    HasBeenRenewed = false,
                },
                new Book
                {
                    Id = 0,
                    Title = "The Picture of Dorian Gray",
                    Author = "Oscar Wilde",
                    Isbn = "9780679783275",
                    Publisher = "Modern Library",
                    PublishDate = DateTime.Now,
                    Description = "The first book in the epic fantasy series A Song of Ice and Fire, which follows the struggles of several noble houses as they fight for control of the Seven Kingdoms.",
                    Genre = "Classic Literature",
                    BookCost = 12.00m,
                    CheckedOutDate = null,
                    DueBackDate = null,
                    HasBeenRenewed = false,
                },
                new Book
                {
                    Id = 0,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = "9780451524935",
                    Publisher = "Grand Central Publishing",
                    PublishDate = DateTime.Now,
                    Description = "A lawyer in a small Southern town defends a black man accused of rape, and his children learn important lessons about racism and morality.",
                    Genre = "Classic Literature",
                    BookCost = 7.99m,
                    CheckedOutDate = null,
                    DueBackDate = null,
                    HasBeenRenewed = false,
                },
            });
        }
        _context.Books.AddRange(books);
        _context.SaveChanges();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

}
