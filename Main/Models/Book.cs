using System;
using System.Collections.Generic;

namespace Main.Models
{
    public class Book : IEquatable<Book>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string PublicationDate { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Price { get; set; }

        public string CheckedOutDate { get; set; }

        public string DueBackDate { get; set; }

        public bool IsCheckedOut => CheckedOutDate != string.Empty;

        public string CheckedOutBy { get; set; }
        public int AvailabilityCount { get; set; }

        #region Distinct

        public bool Equals(Book book)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(book, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, book)) return true;

            //Check whether the products' properties are equal.
            return ISBN.Equals(book.ISBN) && Title.Equals(book.Title);
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int hashBookISBN = ISBN == null ? 0 : ISBN.GetHashCode();

            //Get hash code for the Code field.
            int hashBookTitle = Title == null ? 0 : Title.GetHashCode();

            //Calculate the hash code for the product.
            return hashBookISBN ^ hashBookTitle;
        }

        #endregion
    }
}