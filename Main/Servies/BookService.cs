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
        private readonly string _xmlFilePath = "D:\\Solutions\\LibrarySystem\\Main\\XML\\BookDetails.xml";

        private readonly AccountStore _accountStore;
        private AccountService _accountService => new AccountService(_accountStore);

        public BookService(AccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        public ObservableCollection<Book> GetAllBooks()
        {
            var books = BuildBookListFromXml();

            return new ObservableCollection<Book>(books);
        }

        public void AddBook(Book newBook)
        {
            var doc = XDocument.Load(_xmlFilePath);

            doc.Element("catalog").Add(
                new XElement("book",
                    new XElement("title", newBook.Title),
                    new XElement("author", newBook.Author),
                    new XElement("genre", newBook.Genre),
                    new XElement("price", newBook.Price),
                    new XElement("publish_date", newBook.PublicationDate),
                    new XElement("description", newBook.Summary),
                    new XElement("isbn", newBook.ISBN),
                    new XElement("publisher", newBook.Publisher)));

            doc.Save(_xmlFilePath);
        }

        public ObservableCollection<Book> SearchBooks(string searchString)
        {
            //gets the collection of books
            var books = BuildBookListFromXml();

            //filters down the collection to only display results that match the search term
            books = books.Where(x =>
                x.Author.Contains(searchString) ||
                x.Title.Contains(searchString) ||
                x.ISBN.Contains(searchString)).ToList();

            //returns filtered down results
            return new ObservableCollection<Book>(books);
        }

        public void CheckOutBook(string ISBN)
        {
            //gets collection of books
            var doc = XDocument.Load(_xmlFilePath);

            var singleBook = doc.Descendants("book").Where(x => x.Element("Checked_Out_Date").Value == string.Empty)
                .FirstOrDefault(x => x.Element("isbn").Value == ISBN);
            //changes the value of checked out by 
            singleBook.Element("Checked_Out_By").Value = _accountStore.CurrentUser.LibraryCardNumber;

            //changes the value of checked out date 
            singleBook.Element("Checked_Out_Date").Value = DateTime.Now.ToString();

            //changes the value of due back date, TODO find out how long the default lenght a book can be out for.
            singleBook.Element("Due_Back_Date").Value = DateTime.Now.AddDays(14).ToString();


            _accountService.CheckOutBook(singleBook.Element("isbn").Value, singleBook.Element("title").Value,
                singleBook.Element("checked_out_by").Value, singleBook.Element("due_back_date").Value);

            singleBook.Document.Save(_xmlFilePath);
        }

        public void ReturnBook(string ISBN, string libraryCardNumber)
        {
            var doc = XDocument.Load(_xmlFilePath);

            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            //changes the value of checked out by 
            var singleBook = doc.Descendants("book").Where(x => x.Element("Checked_Out_By").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == ISBN);

            singleBook.Element("Checked_Out_Date").Value = null;
            singleBook.Element("Due_Back_Date").Value = null;
            singleBook.Element("Checked_Out_By").Value = null;

            singleBook.Document.Save(_xmlFilePath);

            _accountService.ReturnBook(ISBN, libraryCardNumber);
        }

        public void RenewBook(string ISBN, string libraryCardNumber)
        {
            var doc = XDocument.Load(_xmlFilePath);

            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            var singleBook = doc.Descendants("book").Where(x => x.Element("Checked_Out_By").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == ISBN);

            var dueBackDate = Convert.ToDateTime(singleBook.Element("Due_Back_Date").Value).AddDays(7).ToString();

            //TODO check how long a book is renewed for.
            singleBook.Element("Due_Back_Date").Value = dueBackDate;

            singleBook.Document.Save(_xmlFilePath);

            _accountService.RenewBook(ISBN, libraryCardNumber, dueBackDate);
        }

        private List<Book> BuildBookListFromXml()
        {
            var doc = XDocument.Load(_xmlFilePath);
            var bookSingle = doc.Descendants("book").Where(x => x.Element("Checked_Out_Date").Value == string.Empty)
                .FirstOrDefault(x => x.Element("isbn").Value == "2");

            var books = doc.Descendants("book").Select(x => new Book
            {
                Title = x.Element("title").Value,
                Author = x.Element("author").Value,
                ISBN = x.Element("isbn").Value,
                Publisher = x.Element("publisher").Value,
                PublicationDate = x.Element("publish_date").Value,
                Summary = x.Element("description").Value,
                Genre = x.Element("genre").Value,
                Price = x.Element("price").Value,
                CheckedOutDate = x.Element("Checked_Out_Date").Value,
                DueBackDate = x.Element("Due_Back_Date").Value,
            }).ToList();

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x => x.IsCheckedOut == false).Count(x => x.ISBN == book.ISBN);

            return books.Distinct().ToList();
        }
    }
}