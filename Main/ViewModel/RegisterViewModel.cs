﻿using System.Windows.Input;
using Main.Commands;
using Main.Servies;
using Main.Stores;

namespace Main.ViewModel
{
    public class RegisterViewModel : BaceViewModel
    {

        public ICommand RegisterCommand { get; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public RegisterViewModel(INavigationService loginNavigationService)
        {

            RegisterCommand = new RegisterCommand(this,loginNavigationService);
        }
    }
}
