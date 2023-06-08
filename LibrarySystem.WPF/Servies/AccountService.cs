using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using LibrarySystem.WPF.Models;
using LibrarySystem.WPF.Stores;

namespace LibrarySystem.WPF.Servies
{
    public class AccountService
    {
        private readonly AccountStore _accountStore;

        private readonly XDocument _userDoc;
        private readonly string _xmlUserFilePath = "XML\\UserDetails.xml";

        public AccountService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _userDoc = XDocument.Load(_xmlUserFilePath);
        }

        private LogService LogService => new LogService();

        public User GetUser(string librarycardnumber, string email)
        {
            //gets a collection of all librarycardnumber numbers ,or emails if that is the login method, and then pulls the one one we are passing though, returning null if there are no matches. 
            var singleUser = GetXmlUser(librarycardnumber, email);

            //if the user dose not exist return null.
            if (singleUser == null)
                return null;

            _accountStore.CurrentUser = BuildUserFromXml(singleUser);

            return _accountStore.CurrentUser;
        }

        public User GetUserDetails(string librarycardnumber, string email)
        {
            //gets a collection of all librarycardnumber numbers ,or emails if that is the login method, and then pulls the one one we are passing though, returning null if there are no matches. 
            var singleUser = GetXmlUser(librarycardnumber, email);

            //if the user dose not exist return null.
            if (singleUser == null)
                return null;

            return BuildUserFromXml(singleUser);
        }


        public List<User> GetAllUsers()
        {
            var users = _userDoc.Descendants("user").Select(BuildUserFromXml).ToList();

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

            LogService.InitialAccountLog(user.LibraryCardNumber, user.Name);
        }

        public void EditUser(User user)
        {
            var singleUser = GetXmlUser(_accountStore.CurrentUser.LibraryCardNumber);

            singleUser.Element("name").Value = user.Name;
            singleUser.Element("email").Value = user.Email;
            singleUser.Element("phone_number").Value = user.PhoneNumber;
            singleUser.Element("account_type").Value = user.AccountType.ToString();

            singleUser.Document.Save(_xmlUserFilePath);


            LogService.AccountLog(string.Empty, user.LibraryCardNumber,
                "Changing account details.\nEdited Account details", "edit_account_logs");
        }


        public void DeleteUser(string libraryCardNumber)
        {
            GetXmlUser(libraryCardNumber).Remove();

            _userDoc.Save(_xmlUserFilePath);

            LogService.AccountLog(string.Empty, libraryCardNumber, "Changing account details.\nAccount Deleted",
                "edit_account_logs");
        }

        public ObservableCollection<Book> GetCheckedOutBooks(string libraryCardNumber)
        {
            var user = GetXmlUser(libraryCardNumber);

            return new ObservableCollection<Book>(user.Descendants("book").Select(x => new Book
            {
                Title = x.Element("title").Value,
                Isbn = x.Element("isbn").Value,
                CheckedOutDate = x.Element("checked_out_date").Value,
                DueBackDate = x.Element("due_back_date").Value
            }));
        }

        public ObservableCollection<Book> GetDueBackBooks(string libraryCardNumber)
        {
            var user = GetXmlUser(libraryCardNumber);

            return new ObservableCollection<Book>(user.Descendants("book")
                .Where(x => DateTime.Parse(x.Element("due_back_date").Value).Date <= DateTime.Now.AddDays(7).Date)
                .Select(x => new Book
                {
                    Title = x.Element("title").Value,
                    Isbn = x.Element("isbn").Value,
                    CheckedOutDate = x.Element("checked_out_date").Value,
                    DueBackDate = x.Element("due_back_date").Value
                }));
        }

        public ObservableCollection<Fine> GetFines(string libraryCardNumber)
        {
            var user = GetXmlUser(libraryCardNumber);

            if (!user.Descendants("fine").Elements().Any())
                return new ObservableCollection<Fine>();

            return new ObservableCollection<Fine>(user.Descendants("fine").Select(x => new Fine
            {
                FineAmount = double.Parse(x.Element("fine_amount")?.Value ?? "0"),
                Reason = x.Element("reason").Value,
                BookTitle = x.Element("book_title").Value,
                PayByDate = DateTime.Parse(x.Element("pay_by_date")?.Value),
                Isbn = x.Element("isbn").Value
            }));
        }

        private User BuildUserFromXml(XElement singleUser)
        {
            return new User
            {
                LibraryCardNumber = singleUser.Element("library_card_number")?.Value,
                Name = singleUser.Element("name")?.Value,
                Email = singleUser.Element("email")?.Value,
                PhoneNumber = singleUser.Element("phone_number")?.Value,
                AccountType = (AccountType)int.Parse(singleUser.Element("account_type").Value)
            };
        }

        private XElement GetXmlUser(string libraryCardNumber)
        {
            return GetXmlUser(libraryCardNumber, string.Empty);
        }

        private XElement GetXmlUser(string lcn, string email)
        {
            return _userDoc.Root
                .Elements("user")
                .SingleOrDefault(x => x.Element("email").Value == email || x.Element("library_card_number").Value == lcn);
        }
    }
}