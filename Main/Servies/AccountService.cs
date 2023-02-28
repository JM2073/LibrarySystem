using System;
using System.Linq;
using System.Xml.Linq;
using Main.Models;
using Main.Stores;

namespace Main.Servies
{
    public class AccountService
    {
        private const string _xmlFilePath = "D:\\Solutions\\LibrarySystem\\Main\\XML\\UserDetails.xml";

        private readonly AccountStore _accountStore;

        public AccountService(AccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        public User GetUser(string librarycardnumber, string email)
        {
            var xdoc = XDocument.Load(_xmlFilePath);

            //gets a collection of all librarycardnumber numbers ,or emails if that is the login method, and then pulls the one one we are passing though, returning null if there are no matches. 
            var singleUser = librarycardnumber != null
                ? xdoc.Descendants("librarycardnumber").SingleOrDefault(x => x.Value == librarycardnumber)?.Parent
                : xdoc.Descendants("email").SingleOrDefault(x => x.Value == email)?.Parent;

            //if the user dose not exist return null.
            if (singleUser == null)
                return null;

            _accountStore.CurrentUser = new User
            {
                LibraryCardNumber = singleUser.Element("librarycardnumber")?.Value,
                Name = singleUser.Element("name")?.Value,
                Email = singleUser.Element("email")?.Value,
                PhoneNumber = singleUser.Element("phonenumber")?.Value,
                Books = singleUser.Descendants("bookscheckedout").Elements().Select(x =>
                    new Book
                    {
                        ISBN = x.Element("isbn").Value,
                        Title = x.Element("title").Value,
                        CheckedOutDate = x.Element("checkedoutdate").Value,
                        DueBackDate = x.Element("duebackdate").Value
                    }).ToList()
            };

            return _accountStore.CurrentUser;
        }

        //TODO make object to store a user thats being created.
        public void AddUser(string name, string email, string phoneNumber)
        {
            var xdoc = XDocument.Load(_xmlFilePath);

            xdoc.Element("users").Add(
                new XElement("user",
                    new XElement("library_card_number", Guid.NewGuid().ToString()),
                    new XElement("name", name),
                    new XElement("email", email),
                    new XElement("phone_number", phoneNumber),
                    new XElement("books_checked_out",
                        new XElement("book"))));

            xdoc.Save(_xmlFilePath);
        }

        #region BookHandlers

        public void CheckOutBook(string isbn, string title, string dueBackDate, string checkedOutBy)
        {
            var xdoc = XDocument.Load(_xmlFilePath);

            xdoc.Descendants("user").SingleOrDefault(x => x.Element("email").Value == _accountStore.CurrentUser.Email)
                .Element("books_checked_out")
                .Add(new XElement("book",
                    new XElement("isbn", isbn),
                    new XElement("title", title),
                    new XElement("checked_out_by", checkedOutBy),
                    new XElement("due_back_date", dueBackDate)));

            xdoc.Save(_xmlFilePath);
        }

        public void ReturnBook(string isbn, string libraryCardNumber)
        {
            var xdoc = XDocument.Load(_xmlFilePath);
            xdoc.Descendants("user").SingleOrDefault(x => x.Element("email").Value == libraryCardNumber)
                .Element("books_checked_out").Elements("book").Where(x => x.Element("isbn").Value == isbn).Remove();
            xdoc.Save(_xmlFilePath);
        }

        #endregion

        public void RenewBook(string isbn, string libraryCardNumber, string dueBackDate)
        {
            var xdoc = XDocument.Load(_xmlFilePath);
            xdoc.Descendants("user").SingleOrDefault(x => x.Element("email").Value == libraryCardNumber)
                .Element("books_checked_out").Elements("book").Single(x => x.Element("isbn").Value == isbn)
                .Element("due_back_date").Value = dueBackDate;

            xdoc.Save(_xmlFilePath);
        }
    }
}