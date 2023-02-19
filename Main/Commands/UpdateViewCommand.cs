using Main.ViewModel;
using System;
using System.Windows.Input;
using _eViews = Main.Enums.Views;

namespace Main.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch ((_eViews)parameter)
            {

                case _eViews.Login:
                    viewModel.SelectedViewModel = new LoginViewModel();
                    break;
                
                case _eViews.Register:
                    viewModel.SelectedViewModel = new RegisterViewModel();
                    break;
                
                default:
                    //TODO IMPLMENT ERROR THAT REPORTS THAT THE VIEWMODEL HAS NOT BEEN ADDED.
                    break;
            
            }

        }
    }
}
