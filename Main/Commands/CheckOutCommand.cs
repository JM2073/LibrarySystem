using System;
using System.Windows;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
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
            bool success = false;
            
            try
            {
                success = _bookService.CheckOutBook(_vm.BookIsbn);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            if(success)
                MessageBox.Show("Book successfully Checked Out");
        }
    }
}