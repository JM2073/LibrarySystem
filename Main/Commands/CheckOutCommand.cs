using System.Windows;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class CheckOutCommand : CommandBace
    {
        private readonly AccountStore _accountStore;
        private readonly CheckOutBookViewModel _vm;

        private readonly BookService _bookService;
        public CheckOutCommand(AccountStore accountStore ,CheckOutBookViewModel vm)
        {
            _accountStore = accountStore;
            _vm = vm;

            _bookService = new BookService(accountStore);
        }
        
        public override void Execute(object parameter)
        {
            bool success = _bookService.CheckOutBook(_vm.BookIsbn);
            MessageBox.Show(success ? "Book successfully Checked Out" : "book not found, please try again.");
        }
    }
}