using System;
using System.Windows;
using LibrarySystem.WPF.Servies;
using LibrarySystem.WPF.Stores;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Commands
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
            try
            {
               _bookService.CheckInBook(_vm.BookIsbn,_accountStore.CurrentUser.LibraryCardNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
                return;
            }
            MessageBox.Show("Book successfully Checked in");
        }
    }
}