using System;
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
        public User GetUser(string librarycardnumber)
        {
            XDocument xdoc = XDocument.Load("D:\\Solutions\\LibrarySystem\\Main\\XML\\UserDetails.xml");

            var singleUser = xdoc.Elements().SingleOrDefault(x => x.Attribute("librarycardnumber")?.Value == librarycardnumber);if (singleUser == null)
                return null;

            // var x = xdoc.Descendants("librarycardnumber").Where(x=>x.Value== "123456789").Single().Parent; try this


            return new User()
            {
                LibraryCardNumber = singleUser.Attribute("librarycardnumber")?.Value,
                Name = singleUser.Attribute("name")?.Value,
                Email = singleUser.Attribute("email")?.Value,
                PhoneNumber = singleUser.Attribute("phonenumber")?.Value,
                NumberOfBooksCheckedOut = int.Parse(singleUser.Attribute("numberofbookscheckedout")?.Value),
              
            };
        }

        public bool AddUser()
        {

            return false;
        }

    }
}
