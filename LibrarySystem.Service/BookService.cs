using System.Collections.ObjectModel;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private const int MAX_BOOK_COUNT = 6;
        private const int CHECK_OUT_TIME = 21;
        private const int RENEW_PERIOD = 7;
        private readonly Guid _currentUserlibraryCardNumber;


        private LibraryDBContextFactory _dbContextFactory;
        private LogService LogService { get; }
        private FineService FineService => new FineService();

        public BookService(Guid currentUserlibraryCardNumber)
        {
            _currentUserlibraryCardNumber = currentUserlibraryCardNumber;
            _dbContextFactory = new LibraryDBContextFactory();
            LogService = new LogService();
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var books = GetBookList(_db.Books.ToList());

                books = books.DistinctBy(x => x.Isbn).ToList();
                return new ObservableCollection<Book>(books);
            }
        }

        public void AddBook(Book newBook)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                newBook.Logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "book Created",
                });
                _db.Books.Add(newBook);

                _db.SaveChanges();
            }
        }

        public void EditBooks(Book book)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var bookList = _db.Books.Include(x => x.Logs).Where(x => x.Isbn == book.Isbn).ToList();

                foreach (var bookSingle in bookList)
                {
                    bookSingle.Title = book.Title;
                    bookSingle.Author = book.Author;
                    bookSingle.Genre = book.Genre;
                    bookSingle.BookCost = book.BookCost;
                    bookSingle.PublishDate = book.PublishDate;
                    bookSingle.Description = book.Description;
                    bookSingle.Publisher = book.Publisher;

                    bookSingle.Logs.Add(new Log
                    {
                        Date = DateTime.Now,
                        Description = "book Edited",
                    });
                }

                _db.SaveChanges();
            }
        }

        public void DeleteBook(string isbn)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var bookList = _db.Books.Include(x => x.Logs).Where(x => x.Isbn == isbn).ToList();

                foreach (var bookSingle in bookList)
                {
                    bookSingle.isArcived = true;
                    bookSingle.Logs.Add(new Log
                    {
                        Date = DateTime.Now,
                        Description = "book arcived",
                    });
                }


                _db.SaveChanges();
            }
        }

        public ObservableCollection<Book> SearchBooks(string searchString)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var books = _db.Books.ToList();
                books = GetBookList(books).Where(x =>
                    x.Author.ToLower().Contains(searchString.ToLower()) ||
                    x.Title.ToLower().Contains(searchString.ToLower()) ||
                    x.Isbn.ToLower().Contains(searchString.ToLower())).ToList();

                return new ObservableCollection<Book>(books);
            }
        }

        public void CheckOutBook(string isbn)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                //ErrorH check to make sure there not null, shouldnt be null but gota handle those errors
                var currentUserEntity = _db.Users.Include(x => x.Books).Include(x => x.Logs).SingleOrDefault(x =>
                    x.LibraryCardNumber == _currentUserlibraryCardNumber);
                var book = _db.Books.FirstOrDefault(x => x.Isbn == isbn);

                //if they have already check out 6 books, dont let them check out any more
                if (currentUserEntity.Books.Count >= MAX_BOOK_COUNT)
                    throw new Exception(
                        "you have checked out all the books you can, contact the librarian to discus your options.");

                //if they already have this book checked out, throw an error.
                if (currentUserEntity.Books.Any(x => x.Isbn == isbn))
                    throw new Exception(
                        "You already have this book checked out.\nplease return your current book first.");

                book.CheckedOutDate = DateTime.Now;
                book.DueBackDate = DateTime.Now.AddDays(CHECK_OUT_TIME);

                currentUserEntity.Books.Add(book);

                currentUserEntity.Logs.Add(new Log
                {
                    BookId = book.Id,
                    Date = DateTime.Now,
                    Description = "book checked out.",
                });

                _db.SaveChanges();
            }
        }

        public void CheckInBook(string isbn, Guid? libraryCardNumber)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                libraryCardNumber = libraryCardNumber ?? _currentUserlibraryCardNumber;
                //ErrorH check to make sure there not null, shouldnt be null but gota handle those errors
                var currentUserEntity = _db.Users
                    .Include(x => x.Books)
                    .Include(x => x.Fines)
                    .Include(x => x.Logs)
                    .SingleOrDefault(x => x.LibraryCardNumber == libraryCardNumber);

                var book = currentUserEntity.Books.SingleOrDefault(x => x.Isbn == isbn);
                var fines = currentUserEntity.Fines.ToList();

                if (fines.Any(x => x.BookId == book.Id))
                    throw new Exception(
                        "you have existing fines for this book. please pay the fines before checking the book back in.");


                currentUserEntity.Books.Remove(book);
                book.CheckedOutDate = null;
                book.DueBackDate = null;


                currentUserEntity.Logs.Add(new Log
                {
                    BookId = book.Id,
                    Date = DateTime.Now,
                    Description = "book checked in.",
                });


                _db.SaveChanges();
            }
        }

        public void RenewBook(string isbn, Guid? libraryCardNumber)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                libraryCardNumber = libraryCardNumber ?? _currentUserlibraryCardNumber;
                //ErrorH check to make sure there not null, shouldnt be null but gota handle those errors
                var currentUserEntity = _db.Users
                    .Include(x => x.Books)
                    .Include(x => x.Fines)
                    .Include(x => x.Logs)
                    .SingleOrDefault(x => x.LibraryCardNumber == libraryCardNumber);

                var book = currentUserEntity.Books.SingleOrDefault(x => x.Isbn == isbn);
                var fines = currentUserEntity.Fines.ToList();

                if (fines.Any(x => x.BookId == book.Id))
                    throw new Exception(
                        "there are existing fines for this book. please make sure the fines are paid before renewing the book.");

                if (book.HasBeenRenewed)
                    throw new Exception(
                        "This book has already been renewed, please contact a librarian about your options.");

                book.DueBackDate = book.DueBackDate?.AddDays(RENEW_PERIOD);
                book.HasBeenRenewed = true;

                currentUserEntity.Logs.Add(new Log
                {
                    BookId = book.Id,
                    Date = DateTime.Now,
                    Description = "book has been renewed.",
                });

                _db.SaveChanges();
            }
        }

        private static List<Book> GetBookList(List<Book> books)
        {
            var bookList = books.Where(x => x.isArcived == false).ToList();

            foreach (var book in bookList)
                book.AvailabilityCount = books.Where(x => x.IsCheckedOut == false).Count(x => x.Isbn == book.Isbn);

            bookList = bookList.DistinctBy(x => x.Isbn).ToList();
            return bookList;
        }
    }
}