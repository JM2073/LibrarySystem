﻿using System;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.WPF.Stores
{
    public class AccountStore
    {
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentAccountChanged?.Invoke();
            }
        }

        public Guid? EditUserId { get; set; }
        
        public bool IsLoggedIn => CurrentUser != null;

        public void LogOut()
        {
            CurrentUser = null;
        }

        public event Action CurrentAccountChanged;

    }
}