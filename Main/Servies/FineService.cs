using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Main.Models;
using Main.Stores;

namespace Main.Servies
{
    public class FineService
    {
        private readonly AccountStore _accountStore;
        private readonly LogService _logService;
        private readonly XDocument _userDoc;
        private readonly string _xmlUserFilePath = "UserDetails.xml";

        public FineService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _userDoc = XDocument.Load(_xmlUserFilePath);
            _logService = new LogService();
        }

        public void AddFine()
        {
            throw new NotImplementedException();
            //TODO add addfine, that a librarian can mange
        }

        public void EditFine()
        {
            //TODO add EditFine, that a librarian can mange
            throw new NotImplementedException();
        }

        public void DeleteFine()
        {
            throw new NotImplementedException();
            //TODO add DeleteFine, that a librarian can mange
        }

        public void PayFine(string isbn, string libraryCardNumber)
        {
            _userDoc.Root.Elements("user")
                .SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber)
                .Element("fines").Elements().Where(x => x.Element("isbn").Value == isbn).Remove();

            _userDoc.Save(_xmlUserFilePath);
        }

        public List<Fine> GetAllFines()
        {
            //TODO add to return all files for all people.
            throw new NotImplementedException();
        }

        public List<Fine> GetUserFines()
        {
            //TODO add to return all fines for one user
            throw new NotImplementedException();
        }

        /// <summary>
        ///     this method runs at the start of the application
        ///     it checks if there are any new fines that need issuing, as well as updates any existing fines if there overdue.
        /// </summary>
        public void CheckFines()
        {
            //This entire method could be a service that runs on a server.
            //This would be the ideal solution especially if the program was using a database 

            var userCollection = _userDoc.Descendants("user").ToList();

            foreach (var singleUser in userCollection)
            {
                foreach (var singleBook in singleUser.Elements("books_checked_out").Elements("book").ToList())
                {
                    if (singleUser.Element("fines").Elements("fine")
                        .Any(x => x.Element("isbn")?.Value == singleBook.Element("isbn")?.Value))
                        continue;

                    var dueBackDate = DateTime.Parse(singleBook.Element("due_back_date").Value);
                    if (dueBackDate < DateTime.Now)
                        if (singleUser.Elements("fines").Any() && singleUser.Elements("fines")
                                .Any(x => x.Element("isbn")?.Value != singleBook.Element("isbn")?.Value))
                        {
                            var fineCost = 0.15 * double.Parse(singleBook.Element("book_cost").Value);

                            var payByDate = DateTime.Now.AddDays(7).ToShortDateString();
                            singleUser.Element("fines").Add(
                                new XElement("fine",
                                    new XElement("fine_amount", fineCost),
                                    new XElement("reason", "Book Late Back"),
                                    new XElement("book_title",singleBook.Element("title").Value),
                                    new XElement("pay_by_date", payByDate),
                                    new XElement("isbn", singleBook.Element("isbn").Value)));
                            singleUser.Document.Save(_xmlUserFilePath);

                            _logService.AccountLog(singleBook.Element("isbn").Value, singleUser.Element("library_card_number").Value,
                                $"a fine was added to this user for book '{singleBook.Element("title").Value}' of amount {fineCost:C}. " +
                                $"they have until {payByDate} to pay",
                                "fines_logs");

                            //TODO send 'email' to the member 
                        }
                }

                foreach (var singleFine in singleUser.Elements("fines").Elements("fine").ToList())
                {
                    if (singleFine.Value == string.Empty)
                        continue;

                    var payByDate = DateTime.Parse(singleFine.Element("pay_by_date").Value);
                    if (payByDate < DateTime.Now)
                    {
                        var currentFine = double.Parse(singleFine.Element("fine_amount").Value);

                        var singleBook = singleUser.Elements("books_checked_out").Elements("book")
                            .SingleOrDefault(x => x.Element("isbn").Value == singleFine.Element("isbn").Value);

                        var bookCost = double.Parse(singleBook.Element("book_cost").Value);

                        if (bookCost <= currentFine)
                        {
                            //TODO send email with final warning about paying the fine.
                           
                            _logService.AccountLog(singleBook.Element("isbn").Value, singleUser.Element("library_card_number").Value,
                                $"the fine for book '{singleBook.Element("title").Value}' was not paid, " +
                                $"the total cost of the book is now overdue," +
                                $"sending final warning and email requesting book back",
                                "fines_logs");
                             continue;
                        }

                        var newFine = currentFine + 0.15 * bookCost;
                        singleFine.Element("fine_amount").Value = newFine.ToString();

                        singleFine.Element("pay_by_date").Value = DateTime.Now.AddDays(7).ToShortDateString();

                        singleFine.Document.Save(_xmlUserFilePath);

                        _logService.AccountLog(singleBook.Element("isbn").Value, singleUser.Element("library_card_number").Value,
                            $"the fine for book '{singleBook.Element("title").Value}' was not paid, the fine of {currentFine:C} is now {newFine:C}",
                            "fines_logs");

                        //TODO send 'email' to the member 
                    }
                }
            }
        }

        public bool CheckForFine(string isbn, string libraryCardNumber)
        {
            return _userDoc.Root.Elements("user").SingleOrDefault(x => x.Element("library_card_number").Value == libraryCardNumber).Elements("fines").Elements().Any(x=>x.Element("isbn").Value == isbn);
        }
    }
}