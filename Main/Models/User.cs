﻿using System.Collections.Generic;

namespace Main.Models
{
    public class User
    {
        public string LibraryCardNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfBooksCheckedOut => Books.Count;
        public AccountType AccountType { get; set; }
        public List<Book> Books { get; set; }
        public List<Fine> Fines { get; set; }
        
    }
    public enum AccountType
    {
        Librarian,
        Member
        
    }
    
}