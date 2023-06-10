using System.Windows;
using System.Windows.Controls;
using LibrarySystem.Domain.Models;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Views
{
    /// <summary>
    ///     Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView 
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private void CheckInBook(object sender, RoutedEventArgs e)
        {
            var vm = (AccountViewModel)this.DataContext;
            vm.CheckInBook(((sender as Button).DataContext as Book).Isbn);
        }

        private void PayFine(object sender, RoutedEventArgs e)
        {
            var vm = (AccountViewModel)this.DataContext;
            vm.PayFine(((sender as Button).DataContext as Fine).Id);
        }

        private void RenewBook(object sender, RoutedEventArgs e)
        {
            var vm = (AccountViewModel)this.DataContext;
            vm.RenewBook(((sender as Button).DataContext as Book).Isbn);
        }
    }
}