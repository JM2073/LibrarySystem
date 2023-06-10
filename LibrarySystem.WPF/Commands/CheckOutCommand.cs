using System;
using System.Windows;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Commands
{
    public class CheckOutCommand : CommandBace
    {
        private readonly CheckOutBookViewModel _vm;

        private readonly BookService _bookService;
        public CheckOutCommand(AccountStore accountStore ,CheckOutBookViewModel vm)
        {
            _vm = vm;

            _bookService = new BookService(accountStore);
        }
        
        public override void Execute(object parameter)
        {
            try
            {
                _bookService.CheckOutBook(_vm.BookIsbn);
                MessageBox.Show("Book successfully Checked Out");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
                
        }
    }
}