using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Main.Models;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchViewModel : BaceViewModel, INotifyPropertyChanged
    {
        private readonly AccountStore _accountStore;
        private BookService _bookService => new BookService(_accountStore);
        private readonly ObservableCollection<Book> _books;
        public IEnumerable<Book> Books => _books;
        public string SearchResultCountString { get; set; }
        public SearchViewModel(SearchStore searchStore, AccountStore accountStore)
        {
            _accountStore = accountStore;
            _books = searchStore.SearchString != null
                ? _bookService.SearchBooks(searchStore.SearchString)
                : _bookService.GetAllBooks();
            
            SearchResultCountString = $"SHOWING '{_books.Count}' RESULTS ";
            SearchResultCountString += searchStore.SearchString != null ? $"FILTERING RESULT BY '{searchStore.SearchString}'" : string.Empty ;
        }
        
        protected String _dynamicText;
        public String DynamicText
        {
            get { return _dynamicText; }
            set { _dynamicText = value;
                RaisePropertyChanged("DynamicText");
                MessageBox.Show("Some Crap");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
      
    }
}