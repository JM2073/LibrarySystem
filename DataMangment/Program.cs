// See https://aka.ms/new-console-template for more information

using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

using (var _db = new LibraryDBContextFactory().CreateDbContext())
{
    Console.WriteLine("Start Migration");
    _db.Database.Migrate();
    Console.WriteLine("Finish Migration");

    Console.WriteLine("adding new user");
    _db.Users.Add(new User
    {
        LibraryCardNumber = "3537ec73-ef63-459d-9ab2-0e3ac48272b9",
        Name = "John Smith",
        PhoneNumber = "555-555-5555",
        Email = "johnsmith@email.com",
        AccountType = AccountType.Librarian,
        Books = new List<Book>{new Book
            {
                Title = "Eleanor Oliphant is Completely Fine",
                Author = "Gail Honeyman",
                Isbn = "9780008272614",
                Publisher = "HarperCollins",
                PublishDate = "01-05-2017",
                Description = "A socially awkward woman navigates life and discovers the power of friendship.",
                Genre = "Contemporary Fiction",
                BookCost = "16.99",
                CheckedOutDate = null,
                DueBackDate = null,
                Logs = new List<Log>{new Log
                    {
                        UserId = null,
                        FineId = null,
                        Date = DateTime.Now,
                        Description = "Book was created",
                        User = null,
                        Fine = null
                    }
                }
            }
        },
        Logs = new List<Log>{new Log
            {
                BookId = null,
                FineId = null,
                Date = DateTime.Now,
                Description = "user was created",
                Book = null,
                Fine = null
            }
        }
            
    });
    Console.WriteLine("new user added   ");
    _db.SaveChanges();


    Console.WriteLine("listing users");
    var users = _db.Users.Include(x=>x.Books).ToList();
    foreach (var user in users)
    {
        Console.WriteLine(user.Name);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.LibraryCardNumber);
        Console.WriteLine();
        if (user.Books.Any())
        {
            Book book = user.Books.FirstOrDefault()!;
                  Console.WriteLine(book.Title);
                    Console.WriteLine(book.Isbn);
        }
  
    }
}

Console.ReadKey();