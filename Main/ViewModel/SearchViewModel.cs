﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Main.Models;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchViewModel : BaceViewModel
    {
        private readonly SearchStore _searchStore;
        private readonly AccountStore _accountStore;
        private BookService BookService => new BookService(_accountStore);

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
                ? BookService.SearchBooks(_searchStore.SearchString)
                : BookService.GetAllBooks();

            ReplaceCollection(booksCollection);

            SearchResultCountString = $"SHOWING '{Books.Count()}' RESULTS ";
            SearchResultCountString += searchStore.SearchString != null ? $"FILTERING RESULT BY '{searchStore.SearchString}'" : string.Empty ;
        }

        public void CheckOutBook(string isbn)
        {
            bool success = false;
            try
            {
                success = BookService.CheckOutBook(isbn);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            if(success) 
                MessageBox.Show( "book checked out");
            
            var booksCollection = _searchStore.SearchString != null
                ? BookService.SearchBooks(_searchStore.SearchString)
                : BookService.GetAllBooks();

            ReplaceCollection(booksCollection);
        }
    }
}