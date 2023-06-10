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
        LibraryCardNumber = new Guid(),
        Name = "john snow",
        PhoneNumber = "685465465",
        Email = "Email",
        AccountType = AccountType.Librarian,
        Books = new List<Book>
        {
            new Book
            {
                Title = "Title",
                Author = "auther",
                Isbn = "isbn",
                Publisher = "publish",
                PublishDate = DateTime.Now,
                Description = "some shite",
                Genre = "anything",
                BookCost = 5,
                CheckedOutDate = DateTime.Now,
                DueBackDate = DateTime.Now.AddMonths(1),
                HasBeenRenewed = false,
            }
        },
    });
    
    _db.SaveChanges();

    var users2 = _db.Users
        .Include(x => x.Fines)
        .Include(x => x.Books)
        .Include(x=>x.Logs)
        .Where(x => x.isArcived == false)
        .ToList();

    foreach (var user in users2)
    {
        foreach (var userBook in user.Books)
        {

            user.Fines.Add(new Fine
            {
                BookId = userBook.Id,
                FineAmount = (decimal)2.52,
                Reason = "book Late Back",
                PayByDate = DateTime.Now,
                IsArchived = false,
                IsPayed = false,
                logs = new List<Log>
                {
                    new Log
                    {
                        Date = DateTime.Now,
                        Description = "fine added to person for late book",
                        isArcived = false
                    }
                }
            });

        }
    }
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