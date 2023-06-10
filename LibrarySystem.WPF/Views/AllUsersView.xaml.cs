using System.Windows;
using System.Windows.Controls;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Views
{
    public partial class AllUsersView : UserControl
    {
        public AllUsersView()
        {
            InitializeComponent();
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            var vm = (AllUsersViewModel)this.DataContext;
            vm.EditUser(((sender as Button).DataContext as DataTableItem).LibraryCardNumber);
        }
    }
}