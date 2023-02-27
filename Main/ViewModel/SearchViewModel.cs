using System.Collections.Generic;
using System.Collections.ObjectModel;
using Main.Models;
using Main.Stores;

namespace Main.ViewModel
{
    public class SearchViewModel : BaceViewModel
    {
        private BookService _bookService => new BookService(new AccountStore());
        private readonly ObservableCollection<Book> _books;
        public IEnumerable<Book> Books => _books;
        public string SearchResultCountString { get; set; }
        public SearchViewModel(SearchStore searchStore)
        {
            _books = searchStore.SearchString != null
                ? _bookService.SearchBooks(searchStore.SearchString)
                : _bookService.GetAllBooks();
            
            SearchResultCountString = $"SHOWING '{_books.Count}' RESULTS ";
            SearchResultCountString += searchStore.SearchString != null ? $"FILTERING RESULT BY '{searchStore.SearchString}'" : string.Empty ;
        }
      
    }
}