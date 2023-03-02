using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Main.Models;

namespace Main.Servies
{
    public class FineService
    {
        private readonly string _xmlUserFilePath = "D:\\Solutions\\LibrarySystem\\Main\\XML\\UserDetails.xml";

        public void AddFine()
        {
            //TODO add addfine, that a librarian can mange
        }

        public void EditFine()
        {
            //TODO add EditFine, that a librarian can mange

        }

        public void DeleteFine()
        {
            //TODO add DeleteFine, that a librarian can mange

        }

        public void PayFine()
        {
            //TODO add payFine, that a member can triger/ librarian manage
        }

        public List<Fine> GetAllFines()
        {
            //TODO add to return all files for all people.
            return new List<Fine>();
        }

        public List<Fine> GetUserFines()
        {
            //TODO add to return all fines for one user
            return new List<Fine>();
        }

        public void CheckFines()
        {
            var userDoc = XDocument.Load(_xmlUserFilePath);

            var userCollection = userDoc.Descendants("user").ToList();

            foreach (var singleUser in userCollection)
            {
                foreach (var singleBook in singleUser.Elements("books_checked_out").Elements("book").ToList())
                {
                    if (singleUser.Element("fines").Elements("fine")
                        .Any(x => x.Element("isbn")?.Value == singleBook.Element("isbn")?.Value))
                        continue;

                    var dueBackDate = DateTime.Parse(singleBook.Element("due_back_date").Value);
                    if (dueBackDate < DateTime.Now)
                    {
                        if (singleUser.Elements("fines").Any() && singleUser.Elements("fines")
                                .Any(x => x.Element("isbn")?.Value != singleBook.Element("isbn")?.Value))
                        {
                            var fineCost = 0.15 * double.Parse(singleBook.Element("book_cost").Value);

                            singleUser.Element("fines").Add(
                                new XElement("fine",
                                    new XElement("fine_amount", fineCost),
                                    new XElement("reason", "Book Late Back"),
                                    new XElement("pay_by_date", DateTime.Now.AddDays(7).ToShortDateString()),
                                    new XElement("isbn", singleBook.Element("isbn").Value)));
                            singleUser.Document.Save(_xmlUserFilePath);

                            //TODO send 'email' to the member 
                        }
                    }
                }

                foreach (var singleFine in singleUser.Elements("fines").Elements("fine").ToList())
                {
                    if (singleFine.Value == string.Empty)
                        continue;
                    
                    var payByDate = DateTime.Parse(singleFine.Element("pay_by_date").Value);
                    if (payByDate < DateTime.Now)
                    {
                        var bookCost = double.Parse(singleUser.Elements("books_checked_out").Elements("book")
                            .SingleOrDefault(x => x.Element("isbn").Value == singleFine.Element("isbn").Value)
                            .Element("book_cost").Value);

                        if (bookCost <= double.Parse(singleFine.Element("fine_amount").Value))
                        {
                            //TODO send email with final warning about paying the fine.
                            continue;
                        }
                        
                        singleFine.Element("fine_amount").Value =
                            (double.Parse(singleFine.Element("fine_amount").Value) + (0.15 * bookCost)).ToString();

                        singleFine.Element("pay_by_date").Value = DateTime.Now.AddDays(7).ToShortDateString();

                        singleFine.Document.Save(_xmlUserFilePath);
                        //TODO send 'email' to the member 
                    }
                }
            }
        }
    }
}