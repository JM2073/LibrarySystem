using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Main.Models;
namespace Main
{
    public class BookService
    {

        public List<Book> GetAllBooks()
        {

            return null; //TODO:
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
            date.InnerText = newBook.PublicationDate.ToShortDateString();

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
