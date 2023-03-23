
using System.Windows;
using System.Windows.Controls;
using Main.Models;
using Main.ViewModel;

namespace Main.Views
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
            var _vm = (AccountViewModel)this.DataContext;
            _vm.CheckInBook(((sender as Button).DataContext as Book).ISBN);
        }

        private void PayFine(object sender, RoutedEventArgs e)
        {
            var _vm = (AccountViewModel)this.DataContext;
            _vm.PayFine(((sender as Button).DataContext as Fine).ISBN);
        }

        private void RenewBook(object sender, RoutedEventArgs e)
        {
            var _vm = (AccountViewModel)this.DataContext;
            _vm.RenewBook(((sender as Button).DataContext as Book).ISBN);
        }
    }
}