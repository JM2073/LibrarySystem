using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using LibrarySystem.WPF.Models;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.Servies
{
    public class BookService
    {
        private readonly string _xmlBookFilePath = "XML\\BookDetails.xml";
        private readonly string _xmlUserFilePath = "XML\\UserDetails.xml";

        private readonly AccountStore _accountStore;
        private readonly XDocument _bookDoc;
        private readonly XDocument _userDoc;
        private LogService LogService => new LogService();
        private FineService FineService => new FineService(_accountStore);

        public BookService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _bookDoc = XDocument.Load(_xmlBookFilePath);
            _userDoc = XDocument.Load(_xmlUserFilePath);
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            var books = BuildBookListFromXml();

            return new ObservableCollection<Book>(books);
        }

        public void AddBook(Book newBook)
        {
            _bookDoc.Element("catalog").Add(
                new XElement("book",
                    new XElement("isbn", newBook.Isbn),
                    new XElement("title", newBook.Title),
                    new XElement("author", newBook.Author),
                    new XElement("genre", newBook.Genre),
                    new XElement("publish_date", newBook.PublishDate),
                    new XElement("publisher", newBook.Publisher),
                    new XElement("book_cost", newBook.BookCost),
                    new XElement("description", newBook.Description)));

            _bookDoc.Save(_xmlBookFilePath);

            LogService.InitialBookLog(newBook.Isbn, newBook.Title);
        }

        public void EditBooks(Book book)
        {
            var bookBookCollection =
                _bookDoc.Descendants("book").Where(x => x.Element("isbn").Value == book.Isbn).ToList();

            foreach (var singleBook in bookBookCollection)
            {
                singleBook.Element("title").Value = book.Title;
                singleBook.Element("author").Value = book.Author;
                singleBook.Element("genre").Value = book.Genre;
                singleBook.Element("book_cost").Value = book.BookCost;
                singleBook.Element("publish_date").Value = book.PublishDate;
                singleBook.Element("description").Value = book.Description;
                singleBook.Element("publisher").Value = book.Publisher;

                singleBook.Document.Save(_xmlBookFilePath);
            }

            var userBookCollection =
                _userDoc.Descendants("book").Where(x => x.Element("isbn").Value == book.Isbn).ToList();

            foreach (var singleBook in userBookCollection)
            {
                singleBook.Element("title").Value = book.Title;
                singleBook.Document.Save(_xmlUserFilePath);
            }

            LogService.BookLog(book.Isbn, string.Empty, "Changing book details.\nEdited book details",
                "edit_book_logs");
        }

        public void DeleteBook(string isbn)
        {
            var bookCollection = _bookDoc.Descendants("book").Where(x => x.Element("checked_out_by").Value == null)
                .Where(x => x.Element("isbn").Value == isbn).ToList();

            foreach (var singleBook in bookCollection)
            {
                singleBook.Remove();
                singleBook.Document.Save(_xmlBookFilePath);
            }

            LogService.BookLog(isbn, string.Empty, "Changing book details.\nEdited book details", "edit_book_logs");
        }

        public ObservableCollection<Book> SearchBooks(string searchString)
        {
            //gets the collection of books
            var books = BuildBookListFromXml();

            //filters down the collection to only display results that match the search term
            books = books.Where(x =>
                x.Author.ToLower().Contains(searchString.ToLower()) ||
                x.Title.ToLower().Contains(searchString.ToLower()) ||
                x.Isbn.ToLower().Contains(searchString.ToLower())).ToList();

            //returns filtered down results
            return new ObservableCollection<Book>(books);
        }

        public bool CheckOutBook(string isbn)
        {
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_date").Value == string.Empty)
                .FirstOrDefault(x => x.Element("isbn").Value == isbn);

            var singleUser = _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("email").Value == _accountStore.CurrentUser.Email);

            if (singleBook == null)
                throw new Exception("Book non");

            if (singleUser.Elements("books_checked_out").Elements()
                .Any(x => x.Element("isbn").Value == singleBook.Element("isbn").Value))
                throw new Exception("You already have this book checked out.\nplease return your current book first.");

            //changes the value of checked out by 
            singleBook.Element("checked_out_by").Value = _accountStore.CurrentUser.LibraryCardNumber;

            //changes the value of checked out date 
            singleBook.Element("checked_out_date").Value = DateTime.Now.ToShortDateString();

            //changes the value of due back date, TODO find out how long the default lenght a book can be out for.
            singleBook.Element("due_back_date").Value = DateTime.Now.AddDays(21).ToShortDateString();


            singleUser.Element("books_checked_out")
                .Add(new XElement("book",
                    new XElement("isbn", singleBook.Element("isbn").Value),
                    new XElement("title", singleBook.Element("title").Value),
                    new XElement("book_cost", singleBook.Element("book_cost").Value),
                    new XElement("has_been_renewed"),
                    new XElement("checked_out_date", singleBook.Element("checked_out_date").Value),
                    new XElement("due_back_date", singleBook.Element("due_back_date").Value)));

            _userDoc.Save(_xmlUserFilePath);

            singleBook.Document.Save(_xmlBookFilePath);

            LogService.BookLog(isbn, _accountStore.CurrentUser.LibraryCardNumber,
                $"book checked out by {_accountStore.CurrentUser.Name}", "check_out_logs");
            return true;
        }

        public void CheckInBook(string isbn, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            if (FineService.CheckForFine(isbn, libraryCardNumber))
                throw new Exception(
                    "you have existing fines for this book. please pay the fines before checking the book back in.");

            var singleUser = _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber);

            //get the book trying to be checked in 
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_by").Value == libraryCardNumber)
                .FirstOrDefault(x => x.Element("isbn").Value == isbn);


            //if the book dose not exist then return false
            if (singleBook == null)
                throw new Exception("Book not found, please check your barcode then try again.");

            //wipe the data for it being checked out.
            singleBook.Element("checked_out_date").Value = string.Empty;
            singleBook.Element("due_back_date").Value = string.Empty;
            singleBook.Element("checked_out_by").Value = string.Empty;


            //clean up user data so that its not in there checked out list.
            singleUser.Element("books_checked_out").Elements("book").Where(x => x.Element("isbn").Value == isbn)
                .Remove();

            LogService.BookLog(isbn, libraryCardNumber, $"book checked back in by {_accountStore.CurrentUser.Name}",
                "check_in_logs");

            _bookDoc.Save(_xmlBookFilePath);
            _userDoc.Save(_xmlUserFilePath);
        }

        public void RenewBook(string isbn, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            var usersBook = _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber)
                .Element("books_checked_out").Elements().Single(x => x.Element("isbn").Value == isbn);

            if (FineService.CheckForFine(isbn, libraryCardNumber))
                throw new Exception(
                    "there are existing fines for this book. please make sure the fines are paid before checking in the book.");

            if (usersBook.Element("has_been_renewed").Value == "True")
                throw new Exception(
                    "This book has already been renewed, please contact a librarian about your options.");

            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_by").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == isbn);

            var dueBackDate = Convert.ToDateTime(singleBook.Element("due_back_date").Value).AddDays(7)
                .ToShortDateString();

            //TODO check how long a book is renewed for.
            singleBook.Element("due_back_date").Value = dueBackDate;


            usersBook.Element("due_back_date").Value = dueBackDate;
            usersBook.Element("has_been_renewed").Value = "True";

            LogService.BookLog(isbn, libraryCardNumber, $"book checkout renewed by {_accountStore.CurrentUser.Name}",
                "renew_book_logs");

            _userDoc.Save(_xmlUserFilePath);
            _bookDoc.Save(_xmlBookFilePath);
        }

        private List<Book> BuildBookListFromXml()
        {
            var books = _bookDoc.Descendants("book").Select(x => new Book
            {
                Title = x.Element("title").Value,
                Author = x.Element("author").Value,
                Isbn = x.Element("isbn").Value,
                Publisher = x.Element("publisher").Value,
                PublishDate = x.Element("publish_date").Value,
                Description = x.Element("description").Value,
                Genre = x.Element("genre").Value,
                BookCost = x.Element("book_cost").Value,
                CheckedOutDate = x.Element("checked_out_date").Value,
                DueBackDate = x.Element("due_back_date").Value
            }).ToList();

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x => x.IsCheckedOut == false).Count(x => x.Isbn == book.Isbn);

            return books.Distinct().ToList();
        }
    }
}