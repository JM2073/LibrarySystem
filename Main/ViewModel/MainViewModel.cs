using Main.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModel
{
    public class MainViewModel : BaceViewModel
    {
        private BaceViewModel _selectedViewModel;

        public BaceViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { 
                
                _selectedViewModel = value;
                OnPropertyChange(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
            UpdateViewCommand =  new UpdateViewCommand(this);
        }



    }
}
