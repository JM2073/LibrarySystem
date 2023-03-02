

using System;
using System.Windows;
using System.Windows.Controls;
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

        private void ChangeText(object sender, RoutedEventArgs e)
        {
            SearchViewModel model = (sender as Button).DataContext as SearchViewModel;
            model.DynamicText = (new Random().Next(0, 100).ToString());
        }
    }
}