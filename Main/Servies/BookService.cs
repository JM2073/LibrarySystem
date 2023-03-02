using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Main.Models;
using Main.Servies;
using Main.Stores;

namespace Main
{
    public class BookService
    {
        private readonly string _xmlBookFilePath = "BookDetails.xml";
        private readonly string _xmlUserFilePath = "UserDetails.xml";

        private readonly AccountStore _accountStore;
        private XDocument _bookDoc;
        private LogService _logService => new LogService();

        public BookService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _bookDoc = XDocument.Load(_xmlBookFilePath);
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
                    new XElement("title", newBook.Title),
                    new XElement("author", newBook.Author),
                    new XElement("genre", newBook.Genre),
                    new XElement("book_cost", newBook.BookCost),
                    new XElement("publish_date", newBook.PublishDate),
                    new XElement("description", newBook.Description),
                    new XElement("isbn", newBook.ISBN),
                    new XElement("publisher", newBook.Publisher)));

            _bookDoc.Save(_xmlBookFilePath);

            _logService.InitialBookLog(newBook.ISBN, newBook.Title);
        }

        public void EditBooks(Book book)
        {
            var bookBookCollection =
                _bookDoc.Descendants("book").Where(x => x.Element("isbn").Value == book.ISBN).ToList();

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

            var userDoc = XDocument.Load(_xmlUserFilePath);
            var userBookCollection =
                userDoc.Descendants("book").Where(x => x.Element("isbn").Value == book.ISBN).ToList();

            foreach (var singleBook in userBookCollection)
            {
                singleBook.Element("title").Value = book.Title;
                singleBook.Document.Save(_xmlUserFilePath);
            }

            _logService.BookLog(book.ISBN, string.Empty, "Changing book details.\nEdited book details",
                "edit_book_logs");
        }

        public void DeleteBook(string ISBN)
        {
            var bookCollection = _bookDoc.Descendants("book").Where(x => x.Element("Checked_Out_By").Value == null)
                .Where(x => x.Element("isbn").Value == ISBN).ToList();

            foreach (var singleBook in bookCollection)
            {
                singleBook.Remove();
                singleBook.Document.Save(_xmlBookFilePath);
            }

            _logService.BookLog(ISBN, string.Empty, "Changing book details.\nEdited book details", "edit_book_logs");
        }

        public ObservableCollection<Book> SearchBooks(string searchString)
        {
            //gets the collection of books
            var books = BuildBookListFromXml();

            //filters down the collection to only display results that match the search term
            books = books.Where(x =>
                x.Author.ToLower().Contains(searchString.ToLower()) ||
                x.Title.ToLower().Contains(searchString.ToLower()) ||
                x.ISBN.ToLower().Contains(searchString.ToLower())).ToList();

            //returns filtered down results
            return new ObservableCollection<Book>(books);
        }

        public void CheckOutBook(string ISBN)
        {
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("Checked_Out_Date").Value == string.Empty)
                .FirstOrDefault(x => x.Element("isbn").Value == ISBN);
            //changes the value of checked out by 
            singleBook.Element("Checked_Out_By").Value = _accountStore.CurrentUser.LibraryCardNumber;

            //changes the value of checked out date 
            singleBook.Element("Checked_Out_Date").Value = DateTime.Now.ToString();

            //changes the value of due back date, TODO find out how long the default lenght a book can be out for.
            singleBook.Element("Due_Back_Date").Value = DateTime.Now.AddDays(14).ToString();

            var userDoc = XDocument.Load(_xmlUserFilePath);

            userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("email").Value == _accountStore.CurrentUser.Email)
                .Element("books_checked_out")
                .Add(new XElement("book",
                    new XElement("isbn", singleBook.Element("isbn").Value),
                    new XElement("title", singleBook.Element("title").Value),
                    new XElement("checked_out_by", singleBook.Element("checked_out_by").Value),
                    new XElement("due_back_date", singleBook.Element("due_back_date").Value)));

            userDoc.Save(_xmlUserFilePath);

            singleBook.Document.Save(_xmlBookFilePath);

            _logService.BookLog(ISBN, _accountStore.CurrentUser.LibraryCardNumber,
                $"book checked out by {_accountStore.CurrentUser.Name}", "check_out_logs");
        }

        public void ReturnBook(string ISBN, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            //changes the value of checked out by 
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("Checked_Out_By").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == ISBN);

            singleBook.Element("Checked_Out_Date").Value = null;
            singleBook.Element("Due_Back_Date").Value = null;
            singleBook.Element("Checked_Out_By").Value = null;

            singleBook.Document.Save(_xmlBookFilePath);

            var userDoc = XDocument.Load(_xmlUserFilePath);
            userDoc.Descendants("user").SingleOrDefault(x => x.Element("email").Value == libraryCardNumber)
                .Element("books_checked_out").Elements("book").Where(x => x.Element("isbn").Value == ISBN).Remove();
            userDoc.Save(_xmlUserFilePath);

            _logService.BookLog(ISBN, libraryCardNumber, $"book checked back in by {_accountStore.CurrentUser.Name}",
                "check_in_logs");
        }

        public void RenewBook(string ISBN, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_by").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == ISBN);

            var dueBackDate = Convert.ToDateTime(singleBook.Element("Due_Back_Date").Value).AddDays(7).ToString();

            //TODO check how long a book is renewed for.
            singleBook.Element("Due_Back_Date").Value = dueBackDate;

            singleBook.Document.Save(_xmlBookFilePath);

            var userDoc = XDocument.Load(_xmlUserFilePath);
            userDoc.Descendants("user").SingleOrDefault(x => x.Element("email").Value == libraryCardNumber)
                .Element("books_checked_out").Elements("book").Single(x => x.Element("isbn").Value == ISBN)
                .Element("due_back_date").Value = dueBackDate;

            userDoc.Save(_xmlUserFilePath);

            _logService.BookLog(ISBN, libraryCardNumber, $"book checkout renewed by {_accountStore.CurrentUser.Name}",
                "renew_book_logs");
        }

        private List<Book> BuildBookListFromXml()
        {
            var books = _bookDoc.Descendants("book").Select(x => new Book
            {
                Title = x.Element("title").Value,
                Author = x.Element("author").Value,
                ISBN = x.Element("isbn").Value,
                Publisher = x.Element("publisher").Value,
                PublishDate = x.Element("publish_date").Value,
                Description = x.Element("description").Value,
                Genre = x.Element("genre").Value,
                BookCost = x.Element("book_cost").Value,
                CheckedOutDate = x.Element("Checked_Out_Date").Value,
                DueBackDate = x.Element("Due_Back_Date").Value
            }).ToList();

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x => x.IsCheckedOut == false).Count(x => x.ISBN == book.ISBN);

            return books.Distinct().ToList();
        }
    }
}