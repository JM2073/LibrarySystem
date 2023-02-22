using System;
using Main.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;

namespace Main
{
    public class BookService 
    {
#if DEBUG
        private string XMLFilePath = "C:\\Users\\reavu\\Documents\\RiderSolutions\\LibrarySystem\\Main\\XML\\BookDetails.xml";
#else
        private string XMLFilePath = "Main/XML/UserDetails.xml";
#endif
        public ObservableCollection<Book> GetAllBooks()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();
            XDocument doc = XDocument.Load(XMLFilePath);

            books =  new ObservableCollection<Book>(doc.Descendants("book").Select(x => new Book
            {
                Title = x.Element("title").Value,
                Author = x.Element("author").Value,
                ISBN = x.Element("isbn").Value,
                Publisher = x.Element("publisher").Value,
                PublicationDate = x.Element("publish_date").Value,
                Summary = x.Element("description").Value,
                Genre = x.Element("genre").Value,
                Price = x.Element("price").Value,
            }).ToList());
            
            return books;
        }

        public void AddBook()
        {
            Book newBook = new Book();
 
            XmlDocument doc = new XmlDocument();
            doc.Load(XMLFilePath);

            XmlElement book = doc.CreateElement("Book");
            XmlElement title = doc.CreateElement("title");
            title.InnerText = newBook.Title;

            XmlElement auth = doc.CreateElement("author");
            auth.InnerText = newBook.Author;

            XmlElement genre = doc.CreateElement("genre");
            genre.InnerText = newBook.Genre;

            XmlElement price = doc.CreateElement("price");
            price.InnerText = newBook.Price.ToString();

            XmlElement date = doc.CreateElement("publish_date");
            date.InnerText = newBook.PublicationDate;

            XmlElement description = doc.CreateElement("description");
            description.InnerText = newBook.Summary;

            XmlElement isbn = doc.CreateElement("isbn");
            isbn.InnerText = newBook.Price.ToString();

            XmlElement publisher = doc.CreateElement("publisher");
            publisher.InnerText = newBook.Publisher;

            XmlElement availableCopies = doc.CreateElement("available_copies");
            availableCopies.InnerText = newBook.AvailableCopies.ToString();

            
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
            ObservableCollection<Book> books = new ObservableCollection<Book>();
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
            }).ToList());
            
            return books;
        }
        
    }
}
