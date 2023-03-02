using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;
using Main.Models;
using Main.Stores;

namespace Main.Servies
{
    public class AccountService
    {
        private readonly string _xmlUserFilePath = "UserDetails.xml";

        private readonly AccountStore _accountStore;
        private LogService _logService => new LogService();

        private XDocument _userDoc;

        public AccountService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _userDoc = XDocument.Load(_xmlUserFilePath);
        }

        public User GetUser(string librarycardnumber, string id)
        {
            
            //gets a collection of all librarycardnumber numbers ,or emails if that is the login method, and then pulls the one one we are passing though, returning null if there are no matches. 
            XElement singleUser = librarycardnumber != null
                ? _userDoc.Root.Elements("user").SingleOrDefault(x => x.Value == librarycardnumber)
                : _userDoc.Root.Elements("user").SingleOrDefault(x => x.Element("email").Value == id || x.Element("library_card_number").Value == id);

            //if the user dose not exist return null.
            if (singleUser == null)
                return null;

            _accountStore.CurrentUser = new User
            {
                LibraryCardNumber = singleUser.Element("library_card_number")?.Value,
                Name = singleUser.Element("name")?.Value,
                Email = singleUser.Element("email")?.Value,
                PhoneNumber = singleUser.Element("phone_number")?.Value,
                AccountType = (AccountType)int.Parse(singleUser.Element("account_type").Value),
                Books = singleUser.Element("books_checked_out").Elements().Select(y =>
                    new Book
                    {
                        ISBN = y.Element("isbn").Value,
                        Title = y.Element("title").Value,
                        BookCost = y.Element("book_cost").Value,
                        CheckedOutDate = y.Element("checked_out_date").Value,
                        DueBackDate = y.Element("due_back_date").Value
                    }).ToList(),
                Fines = singleUser.Descendants("fines").Elements().Select(x =>
                    new Fine
                    {
                        FineAmount = double.Parse(x.Element("fine_amount").Value),
                        Reason = x.Element("reason").Value,
                        PayByDate = DateTime.Parse(x.Element("pay_by_date").Value),
                        ISBN = x.Element("isbn").Value
                    }).ToList()
            };

            return _accountStore.CurrentUser;
        }

        public List<User> GetAllUsers()
        {
            var users = _userDoc.Descendants("user").Select(x => new User
            {
                LibraryCardNumber = x.Element("library_card_number")?.Value,
                Name = x.Element("name")?.Value,
                Email = x.Element("email")?.Value,
                PhoneNumber = x.Element("phone_number")?.Value,
                AccountType = (AccountType)int.Parse(x.Element("account_type").Value),
                Books = x.Element("books_checked_out").Elements().Select(y =>
                    new Book
                    {
                        ISBN = y.Element("isbn").Value,
                        Title = y.Element("title").Value,
                        BookCost = y.Element("book_cost").Value,
                        CheckedOutDate = y.Element("checked_out_date").Value,
                        DueBackDate = y.Element("due_back_date").Value
                    }).ToList(),
                Fines = x.Element("fines").Elements().Select(y =>
                    new Fine
                    {
                        FineAmount = double.Parse(y.Element("fine_amount").Value),
                        Reason = y.Element("reason").Value,
                        PayByDate = DateTime.Parse(y.Element("pay_by_date").Value),
                        ISBN = y.Element("isbn").Value
                    }).ToList()
            }).ToList();

            return users;
        }


        public void AddUser(User user)
        {
            _userDoc.Element("users").Add(
                new XElement("user",
                    new XElement("library_card_number", user.LibraryCardNumber),
                    new XElement("name", user.Name),
                    new XElement("email", user.Email),
                    new XElement("phone_number", user.PhoneNumber),
                    new XElement("account_type", user.AccountType.GetHashCode()),
                    new XElement("books_checked_out"),
                    new XElement("fines")
                ));

            _userDoc.Save(_xmlUserFilePath);

            _logService.InitialAccountLog(user.LibraryCardNumber, user.Name);
        }

        public void EditUser(User user)
        {
            var singleUser = _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("email").Value == _accountStore.CurrentUser.Email);

            singleUser.Element("name").Value = user.Name;
            singleUser.Element("email").Value = user.Email;
            singleUser.Element("phone_number").Value = user.PhoneNumber;
            singleUser.Element("account_type").Value = user.AccountType.ToString();

            singleUser.Document.Save(_xmlUserFilePath);


            _logService.AccountLog(string.Empty, user.LibraryCardNumber,
                "Changing account details.\nEdited Account details", "edit_account_logs");
        }

        public void DeleteUser(string libraryCardNumber)
        {
            _userDoc.Descendants("user")
                .SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber).Remove();

            _userDoc.Save(_xmlUserFilePath);

            _logService.AccountLog(string.Empty, libraryCardNumber, "Changing account details.\nAccount Deleted",
                "edit_account_logs");
        }
    }
}