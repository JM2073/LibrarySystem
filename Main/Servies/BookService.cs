using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Main.Models;
using Main.Stores;

namespace Main
{
    public class BookService
    {
        private readonly AccountStore _currentUser;

        public BookService(AccountStore currentUser)
        {
            _currentUser = currentUser;
        }

#if DEBUG
        private readonly string XMLFilePath =
            "D:\\Solutions\\LibrarySystem\\Main\\XML\\BookDetails.xml";
#else
        private string XMLFilePath = "Main/XML/UserDetails.xml";
#endif
        public ObservableCollection<Book> GetAllBooks()
        {
            var doc = XDocument.Load(XMLFilePath);

            var books = new ObservableCollection<Book>(doc.Descendants("book").Select(x => new Book
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
            }).ToList());

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x=>x.IsCheckedOut == false).Count(x => x.ISBN == book.ISBN);
            
            
            return new ObservableCollection<Book>(books.Distinct().ToList());
        }

        public void AddBook(Book newBook)
        {
            var doc = new XmlDocument();
            doc.Load(XMLFilePath);

        
            var book = doc.CreateElement("Book");
            var title = doc.CreateElement("title");
            title.InnerText = newBook.Title;

            var auth = doc.CreateElement("author");
            auth.InnerText = newBook.Author;

            var genre = doc.CreateElement("genre");
            genre.InnerText = newBook.Genre;

            var price = doc.CreateElement("price");
            price.InnerText = newBook.Price;

            var date = doc.CreateElement("publish_date");
            date.InnerText = newBook.PublicationDate;

            var description = doc.CreateElement("description");
            description.InnerText = newBook.Summary;

            var isbn = doc.CreateElement("isbn");
            isbn.InnerText = newBook.Price;

            var publisher = doc.CreateElement("publisher");
            publisher.InnerText = newBook.Publisher;
        
            book.AppendChild(title);
            book.AppendChild(auth);
            book.AppendChild(genre);
            book.AppendChild(price);
            book.AppendChild(date);
            book.AppendChild(description);

            doc.DocumentElement.AppendChild(book);
            doc.Save(XMLFilePath);
        }

        public ObservableCollection<Book> SearchBooks(string searchString)
        {
            var books = new ObservableCollection<Book>();
            var doc = XDocument.Load(XMLFilePath);
            
            books = new ObservableCollection<Book>(doc.Descendants("book").Where(x =>
                x.Element("author").Value.Contains(searchString) ||
                x.Element("title").Value.Contains(searchString) ||
                x.Element("isbn").Value.Contains(searchString)).Select(x => new Book
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
                DueBackDate = x.Element("Due_Back_Date").Value
            }).ToList());

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x=>x.IsCheckedOut == false).Count(x => x.ISBN == book.ISBN);
            
            return new ObservableCollection<Book>(books.Distinct().ToList());
        }

        public void CheckOutBook()
        {
            
        }
    }
}