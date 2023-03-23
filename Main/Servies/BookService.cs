﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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
        private XDocument _userDoc;
        private LogService _logService => new LogService();

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
                    new XElement("isbn", newBook.ISBN),
                    new XElement("title", newBook.Title),
                    new XElement("author", newBook.Author),
                    new XElement("genre", newBook.Genre),
                    new XElement("publish_date", newBook.PublishDate),
                    new XElement("publisher", newBook.Publisher),
                    new XElement("book_cost", newBook.BookCost),
                    new XElement("description", newBook.Description)));

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

            var userBookCollection =
                _userDoc.Descendants("book").Where(x => x.Element("isbn").Value == book.ISBN).ToList();

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
            var bookCollection = _bookDoc.Descendants("book").Where(x => x.Element("checked_out_by").Value == null)
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

        public bool CheckOutBook(string ISBN)
        {
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_date").Value == string.Empty)
                .FirstOrDefault(x => x.Element("isbn").Value == ISBN);
            
            var singleUser = _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("email").Value == _accountStore.CurrentUser.Email);

            if (singleBook == null)
                throw new Exception("Book non");

            if (singleUser.Elements("books_checked_out").Elements().Any(x => x.Element("isbn").Value == singleBook.Element("isbn").Value))
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
                    new XElement("checked_out_date", singleBook.Element("checked_out_date").Value),
                    new XElement("due_back_date", singleBook.Element("due_back_date").Value)));

            _userDoc.Save(_xmlUserFilePath);

            singleBook.Document.Save(_xmlBookFilePath);

            _logService.BookLog(ISBN, _accountStore.CurrentUser.LibraryCardNumber,
                $"book checked out by {_accountStore.CurrentUser.Name}", "check_out_logs");
            return true;
        }

        public bool CheckInBook(string ISBN, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            var singleUser = _userDoc.Descendants("user").SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber);
            
            //get the book trying to be checked in 
            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_by").Value == libraryCardNumber)
                .FirstOrDefault(x => x.Element("isbn").Value == ISBN);

            
            //if the book dose not exist then return false
            if (singleBook == null)
                return false;
            
            //wipe the data for it being checked out.
            singleBook.Element("checked_out_date").Value = string.Empty;
            singleBook.Element("due_back_date").Value = string.Empty;
            singleBook.Element("checked_out_by").Value = string.Empty;

            singleBook.Document.Save(_xmlBookFilePath);

            //clean up user data so that its not in there checked out list.
            singleUser.Element("books_checked_out").Elements("book").Where(x => x.Element("isbn").Value == ISBN).Remove();
            _userDoc.Save(_xmlUserFilePath);

            _logService.BookLog(ISBN, libraryCardNumber, $"book checked back in by {_accountStore.CurrentUser.Name}",
                "check_in_logs");
            
            //the book was successfully checked in, return true
            return true;
        }

        public void RenewBook(string ISBN, string libraryCardNumber)
        {
            libraryCardNumber = libraryCardNumber ?? _accountStore.CurrentUser.LibraryCardNumber;

            var singleBook = _bookDoc.Descendants("book")
                .Where(x => x.Element("checked_out_by").Value == libraryCardNumber)
                .SingleOrDefault(x => x.Element("isbn").Value == ISBN);

            var dueBackDate = Convert.ToDateTime(singleBook.Element("due_back_date").Value).AddDays(7)
                .ToShortDateString();

            //TODO check how long a book is renewed for.
            singleBook.Element("due_back_date").Value = dueBackDate;

            singleBook.Document.Save(_xmlBookFilePath);

            _userDoc.Descendants("user").SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber)
                .Element("books_checked_out").Elements().Single(x => x.Element("isbn").Value == ISBN)
                .Element("due_back_date").Value = dueBackDate;

            _userDoc.Save(_xmlUserFilePath);

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
                CheckedOutDate = x.Element("checked_out_date").Value,
                DueBackDate = x.Element("due_back_date").Value
            }).ToList();

            foreach (var book in books)
                book.AvailabilityCount = books.Where(x => x.IsCheckedOut == false).Count(x => x.ISBN == book.ISBN);

            return books.Distinct().ToList();
        }
    }
}
