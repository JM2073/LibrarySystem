using System.Windows;
using Main.Servies;
using Main.Stores;
using Main.ViewModel;

namespace Main.Commands
{
    public class CheckInCommand : CommandBace
    {
        private readonly AccountStore _accountStore;
        private readonly CheckInBookViewModel _vm;

        private readonly BookService _bookService;
        private readonly FineService _fineService;
        public CheckInCommand(AccountStore accountStore, CheckInBookViewModel vm)
        {
            _accountStore = accountStore;
            _vm = vm;

            _bookService = new BookService(accountStore);
            _fineService = new FineService(accountStore);
        }
        
        public override void Execute(object parameter)
        {
            bool FineCheckSucess = _fineService.CheckForFine(_vm.BookIsbn,_accountStore.CurrentUser.LibraryCardNumber);
            
            bool success = _bookService.CheckInBook(_vm.BookIsbn,_accountStore.CurrentUser.LibraryCardNumber);
            MessageBox.Show(success ? "Book successfully Checked in" : "Check in Failed, please try again.");
        }
    }
}