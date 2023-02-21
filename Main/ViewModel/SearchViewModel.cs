using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Models;

namespace Main.ViewModel
{
    public class SearchViewModel : BaceViewModel
    {

        private readonly BookService _bookService;

        private readonly ObservableCollection<Book> _books;
        public IEnumerable<Book> Books => _books;
        
        public SearchViewModel()
        {
            _bookService = new BookService();
            _books = new ObservableCollection<Book>();

          _books =  _bookService.GetAllBooks();

        }

    }
}
