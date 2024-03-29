﻿using System.Windows;
using System.Windows.Controls;
using LibrarySystem.Domain.Models;
using LibrarySystem.WPF.ViewModel;

namespace LibrarySystem.WPF.Views
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
            var vm = (SearchViewModel)this.DataContext;
            vm.CheckOutBook(((sender as Button).DataContext as Book).Isbn);
        }
    }
}