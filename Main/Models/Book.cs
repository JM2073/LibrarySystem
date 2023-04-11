using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models
{
    public class Book : IEquatable<Book>, INotifyPropertyChanged
    {
        private string _title;
        private string _author;
        private string _isbn;
        private string _publisher;
        private string _publishDate;
        private string _description;
        private string _genre;
        private string _bookCost;
        private string _checkedOutDate;
        private string _dueBackDate;
        private string _checkedOutBy;
        private int _availabilityCount;

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                if (value == _author) return;
                _author = value;
                OnPropertyChanged();
            }
        }

        public string Isbn
        {
            get => _isbn;
            set
            {
                if (value == _isbn) return;
                _isbn = value;
                OnPropertyChanged();
            }
        }

        public string Publisher
        {
            get => _publisher;
            set
            {
                if (value == _publisher) return;
                _publisher = value;
                OnPropertyChanged();
            }
        }

        public string PublishDate
        {
            get => _publishDate;
            set
            {
                if (value == _publishDate) return;
                _publishDate = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                if (value == _genre) return;
                _genre = value;
                OnPropertyChanged();
            }
        }

        public string BookCost
        {
            get => _bookCost;
            set
            {
                if (value == _bookCost) return;
                _bookCost = value;
                OnPropertyChanged();
            }
        }

        public string CheckedOutDate
        {
            get => _checkedOutDate;
            set
            {
                if (value == _checkedOutDate) return;
                _checkedOutDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckedOut));
            }
        }

        public string DueBackDate
        {
            get => _dueBackDate;
            set
            {
                if (value == _dueBackDate) return;
                _dueBackDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckedOut => CheckedOutDate != string.Empty;

        public string CheckedOutBy
        {
            get => _checkedOutBy;
            set
            {
                if (value == _checkedOutBy) return;
                _checkedOutBy = value;
                OnPropertyChanged();
            }
        }

        public int AvailabilityCount
        {
            get => _availabilityCount;
            set
            {
                if (value == _availabilityCount) return;
                _availabilityCount = value;
                OnPropertyChanged();
            }
        }

        #region Distinct

        public bool Equals(Book book)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(book, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, book)) return true;

            //Check whether the products' properties are equal.
            return Isbn.Equals(book.Isbn) && Title.Equals(book.Title);
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int hashBookIsbn = Isbn == null ? 0 : Isbn.GetHashCode();

            //Get hash code for the Code field.
            int hashBookTitle = Title == null ? 0 : Title.GetHashCode();

            //Calculate the hash code for the product.
            return hashBookIsbn ^ hashBookTitle;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}