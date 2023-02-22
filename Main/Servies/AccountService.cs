﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Main.Models;

namespace Main.Servies
{
    public class AccountService 
    {

        
#if DEBUG
        private string XMLFilePath = "C:\\Users\\reavu\\Documents\\RiderSolutions\\LibrarySystem\\Main\\XML\\UserDetails.xml";
#else
        private string XMLFilePath = "Main/XML/UserDetails.xml";
#endif
        public User GetUser(string librarycardnumber, string email)
        {
            XDocument xdoc = XDocument.Load(XMLFilePath);


            XElement singleUser;
            //gets a collection of all librarycardnumber numbers ,or emails if that is the login method, and then pulls the one one we are passing though, returning null if there are no matches. 
            singleUser = librarycardnumber != null ? xdoc.Descendants("librarycardnumber").SingleOrDefault(x => x.Value == librarycardnumber)?.Parent : xdoc.Descendants("email").SingleOrDefault(x => x.Value == email)?.Parent;
            
            //if the user dose not exist return null.
           if (singleUser == null)
                return null;

           User user = new User()
            {
                LibraryCardNumber = singleUser.Element("librarycardnumber")?.Value,
                Name = singleUser.Element("name")?.Value,
                Email = singleUser.Element("email")?.Value,
                PhoneNumber = singleUser.Element("phonenumber")?.Value,
                NumberOfBooksCheckedOut = int.Parse(singleUser.Element("numberofbookscheckedout")?.Value),
                Books =   singleUser.Descendants("bookscheckedout").Elements().Select(x=>
                    new Book
                    {
                        ISBN = x.Element("isbn").Value,
                        Title = x.Element("title").Value,
                        CheckedOutDate = x.Element("checkedoutdate").Value,
                        DueBackDate = x.Element("duebackdate").Value
                    }).ToList()
            };

           return user;
        }

        //TODO make object to store a user thats being created.
        public void AddUser(string _name, string _email, string _phoneNumber)
        {
            var doc = new XmlDocument();
            doc.Load(XMLFilePath);

            //TODO create uniqe ID for the librarycard numbers, for now using Guids
            
            XmlElement user = doc.CreateElement("user");
            
            XmlElement libraryCardNumber = doc.CreateElement("librarycardnumber");
            libraryCardNumber.InnerText = Guid.NewGuid().ToString();
            
            XmlElement name = doc.CreateElement("name");
            name.InnerText = _name;

            XmlElement email = doc.CreateElement("email");
            email.InnerText = _email;
            
            XmlElement phoneNumber = doc.CreateElement("phonenumber");
            phoneNumber.InnerText = _phoneNumber;

            XmlElement numOfBooks = doc.CreateElement("numberofbookscheckedout");
            numOfBooks.InnerText = "0";

            XmlElement books = doc.CreateElement("bookscheckedout");
            
            
            user.AppendChild(libraryCardNumber);
            user.AppendChild(name);
            user.AppendChild(email);
            user.AppendChild(phoneNumber);
            user.AppendChild(numOfBooks);
            user.AppendChild(books);

            doc.DocumentElement.AppendChild(user);
            doc.Save(XMLFilePath);
        }
    }
}
