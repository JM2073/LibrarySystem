using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using LibrarySystem.WPF.Stores;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.WPF.Servies
{
    public class LogService
    {
        private readonly string _xmlLogFilePath = "XML\\LogDetails.xml";
        private readonly XDocument _logDoc;
        private LibraryDBContextFactory _dbContextFactory;

        public LogService()
        {
            _dbContextFactory = new LibraryDBContextFactory();
            _logDoc = XDocument.Load(_xmlLogFilePath);
        }

        
        
        public List<Log> GetAllLogs()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Logs.ToList();
            }
        }

        public List<Log> GetAllBookLogs()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Logs.Where(x => x.BookId != null).ToList();
            }
        }

        public List<Log> GetAllAccountLogs()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Logs.Where(x => x.UserId != null).ToList();
            }
        }
        public List<Log> GetAllFineLogs()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Logs.Where(x => x.FineId != null).ToList();
            }
        }

        public Log AddLog(string description, int? bookId = null, int? userId = null, int? fineId = null)
        {
            return new Log
            {
                BookId = bookId,
                UserId = userId,
                FineId = fineId,
                Date = DateTime.Now,
                Description = description,
            };
        }

        public void InitialLogs()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                _db.Users.Include(x=>x.Logs).ToList().ForEach(x=>x.Logs.Add(AddLog($"new account added to the system with the name '{x.Name}' and the library card number '{x.LibraryCardNumber}'")));
                _db.Books.Include(x=>x.Logs).ToList().ForEach(x=>x.Logs.Add(AddLog($"new book added to the system with the title '{x.Title}' and the isbn {x.Isbn}")));

                _db.SaveChanges();
            }
        }
    }
}