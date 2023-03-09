

using System;
using System.Windows;
using System.Windows.Controls;
using Main.Models;
using Main.ViewModel;

namespace Main.Views
{
    /// <summary>
    ///     Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void CheckOutBook(object sender, RoutedEventArgs e)
        {
            var _vm = (SearchViewModel)this.DataContext;
            _vm.CheckOutBook(((sender as Button).DataContext as Book).ISBN);
        }
    }
}