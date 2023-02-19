using Main.ViewModel;
using System.Windows;
using _eView = Main.Enums.Views;


namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            var viewModel = (MainViewModel)DataContext;

            if (viewModel.UpdateViewCommand.CanExecute(null))
                viewModel.UpdateViewCommand.Execute(_eView.Login);
        }
    }
}
