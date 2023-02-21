using System;
using Main.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Xml;
namespace Main
{
    public class BookService
    {

        public ObservableCollection<Book> GetAllBooks()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();
            var xmlDocument = new XmlDocument();
            xmlDocument.Load("D:\\Solutions\\LibrarySystem\\Main\\XML\\BookDetails.xml");

            XmlNodeList nodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/book"); 

            foreach (XmlNode node in nodeList)
            {
                var item = new Book();
                item.Author = node.SelectSingleNode("author")?.InnerText;
                item.Title = node.SelectSingleNode("title")?.InnerText;
                item.Genre = node.SelectSingleNode("genre")?.InnerText;
                item.Price = node.SelectSingleNode("price")?.InnerText;
                item.PublicationDate = node.SelectSingleNode("publish_date")?.InnerText;
                item.Summary = node.SelectSingleNode("description")?.InnerText;
                books.Add(item);
            }

            return books;
        }

        public void AddBook()
        {

            Book newBook = new Book();
            //    newBook.Title = txtTitle.Text;
            //    newBook.auther = txtAuthor.Text;
            //    newBook.genre= txtGenre.Text;
            //    newBook.price= txtPrice.Text;
            //    newBook.publish_date= txtDate.Text;
            //    newBook.description= txtDescription.Text;

            XmlDocument doc = new XmlDocument();
            doc.Load("Books.XML");

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

            book.AppendChild(title);
            book.AppendChild(auth);
            book.AppendChild(genre);
            book.AppendChild(price);
            book.AppendChild(date);
            book.AppendChild(description);

            doc.DocumentElement.AppendChild(book);
            doc.Save("Books.XML");


        }

    }
}
