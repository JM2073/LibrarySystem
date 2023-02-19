using Main.ViewModel;
using System.Windows;
using System.Windows.Controls;
using _eView = Main.Enums.Views;


namespace Main.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainViewModel)DataContext;

            if (viewModel.UpdateViewCommand.CanExecute(null))
                viewModel.UpdateViewCommand.Execute(_eView.Register);
        }
    }
}
