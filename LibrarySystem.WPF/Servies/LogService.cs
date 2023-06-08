using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.Servies
{
    public class LogService
    {
        private readonly string _xmlLogFilePath =  "XML\\LogDetails.xml";
        private readonly XDocument _logDoc;

        public LogService()
        {
            _logDoc = XDocument.Load(_xmlLogFilePath);
        }

        public void BookLog(string isbn, string libraryCardNumber, string logDescription, string logPath)
        {
            var logDocPath = _logDoc.Descendants("book_log").SingleOrDefault(x => x.Element("isbn").Value == isbn)
                .Element(logPath);

            AddLog(isbn, libraryCardNumber, logDescription, logDocPath);
        }

        public void AccountLog(string isbn, string libraryCardNumber, string logDescription, string logPath)
        {
            var logDocPath = _logDoc.Descendants("account_log")
                .SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber)
                .Element(logPath);

            AddLog(isbn, libraryCardNumber, logDescription, logDocPath);
        }


        public List<XElement> GetAllBookLogs()
        {
            var logDoc = XDocument.Load(_xmlLogFilePath);
            return logDoc.Descendants("book_log").ToList();
        }

        public List<XElement> GetAllAccountLogs()
        {
            var logDoc = XDocument.Load(_xmlLogFilePath);
            return logDoc.Descendants("account_log").ToList();
        }

        public void InitialAccountLog(string libraryCardNumber, string name)
        {
            _logDoc.Element("logs").Element("account_logs").Add(
                new XElement("account_log",
                    new XElement("library_card_number", libraryCardNumber),
                    new XElement("name", name),
                    new XElement("fines_logs"),
                    new XElement("edit_account_logs",
                        new XElement("log",
                            new XElement("date", DateTime.Now.ToString()),
                            new XElement("isbn"),
                            new XElement("library_card_number", libraryCardNumber),
                            new XElement("description",
                                $"new account added to the system with the name '{name}' and the library card number '{libraryCardNumber}'")))));

            _logDoc.Save(_xmlLogFilePath);
        }

        public void InitialBookLog(string isbn, string title)
        {
            _logDoc.Element("logs").Element("book_logs").Add(
                new XElement("book_log",
                    new XElement("isbn", isbn),
                    new XElement("title", title),
                    new XElement("check_out_logs"),
                    new XElement("check_in_logs"),
                    new XElement("renew_book_logs"),
                    new XElement("edit_book_logs",
                        new XElement("log",
                            new XElement("date", DateTime.Now.ToString()),
                            new XElement("isbn", isbn),
                            new XElement("library_card_number"),
                            new XElement("description",
                              $"new book added to the system with the title '{title}' and the isbn {isbn}")))));

            _logDoc.Save(_xmlLogFilePath);
        }

        private void AddLog(string isbn, string libraryCardNumber, string logDescription, XElement logPath)
        {
            logPath.Add(
                new XElement("log",
                    new XElement("date", DateTime.Now.ToString()),
                    new XElement("isbn", isbn),
                    new XElement("library_card_number", libraryCardNumber),
                    new XElement("description", logDescription)));

            logPath.Document.Save(_xmlLogFilePath);
        }

        public void InitialLogs()
        {
            var us = new AccountService(new AccountStore());
            var users= us.GetAllUsers();

            foreach (var user in users)
            {
                InitialAccountLog(user.LibraryCardNumber,user.Name);
            }

            var bs = new BookService(new AccountStore());
            var books = bs.GetAllBooks();

            foreach (var book in books)
            {
                InitialBookLog(book.Isbn,book.Title);
            }
        }
    }
}