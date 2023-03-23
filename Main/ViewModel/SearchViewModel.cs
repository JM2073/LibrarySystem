using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Main.Commands;
using Main.Models;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchViewModel : BaceViewModel, INotifyPropertyChanged
    {
        private readonly SearchStore _searchStore;
        private readonly AccountStore _accountStore;
        private BookService _bookService => new BookService(_accountStore);

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChange(nameof(Books));
            }
        }
        public void ReplaceCollection(IEnumerable<Book> newItems)
        {
            Books = new ObservableCollection<Book>(newItems);
        }

        public string SearchResultCountString { get; set; }
        public SearchViewModel(SearchStore searchStore, AccountStore accountStore)
        {
            _searchStore = searchStore;
            _accountStore = accountStore;

            var booksCollection = _searchStore.SearchString != null
                ? _bookService.SearchBooks(_searchStore.SearchString)
                : _bookService.GetAllBooks();

            ReplaceCollection(booksCollection);

            SearchResultCountString = $"SHOWING '{Books.Count()}' RESULTS ";
            SearchResultCountString += searchStore.SearchString != null ? $"FILTERING RESULT BY '{searchStore.SearchString}'" : string.Empty ;
        }

        public void CheckOutBook(string isbn)
        {
            MessageBox.Show(_bookService.CheckOutBook(isbn) ? "book checked out" : "these has been an error, please try again.");
            
            var booksCollection = _searchStore.SearchString != null
                ? _bookService.SearchBooks(_searchStore.SearchString)
                : _bookService.GetAllBooks();

            ReplaceCollection(booksCollection);
        }
    }
}