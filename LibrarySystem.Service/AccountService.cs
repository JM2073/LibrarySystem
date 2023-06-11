using System.Collections.ObjectModel;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service
{
    public class AccountService
    {

        private LibraryDBContextFactory _dbContextFactory;
        private LogService LogService { get; } 


        public AccountService()
        {
            _dbContextFactory = new LibraryDBContextFactory();
            LogService = new LogService();
        }


        public User? GetUser(Guid? librarycardnumber, string email)
        {
            User? user;
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                user = _db.Users.Where(x => x.isArcived == false)
                    .SingleOrDefault(x => x.LibraryCardNumber == librarycardnumber || x.Email == email);
            }
            if (user == null)
                return null;

            return user;

        }

        public List<User> GetAllActiveUsers()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Users.Where(x => x.isArcived == false).ToList();
            }
        }


        public void AddUser(User user)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                user.Logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "User Added",
                });

                _db.Users.Add(user);
                _db.SaveChanges();
            } 
        }

        public void EditUser(User user)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var obj = _db.Users.Include(x=>x.Logs).SingleOrDefault(x => x.Id == user.Id);

                obj.LibraryCardNumber = user.LibraryCardNumber;
                obj.Email = user.Email;
                obj.Name = user.Name;
                obj.PhoneNumber = user.PhoneNumber;
                obj.Logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "user Edited",
                });
                
                
                _db.SaveChanges();
            }
        }


        public void DeleteUser(int id)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var obj = _db.Users.Include(x=>x.Logs).SingleOrDefault(x => x.Id == id);

                obj.isArcived = true;
                obj.Logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "user arcived",
                });
                _db.SaveChanges();
            }
        }

        public ObservableCollection<Book> GetCheckedOutBooks(int id)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var obj = _db.Users.Include(x=>x.Books).SingleOrDefault(x => x.Id == id);

                return new ObservableCollection<Book>(obj.Books.Where(x=>x.IsCheckedOut).ToList());
            }
        }

        public ObservableCollection<Book> GetDueBackBooks(int id)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var obj = _db.Users.Include(x=>x.Books).SingleOrDefault(x => x.Id == id);

                return new ObservableCollection<Book>(obj.Books.Where(x=>x.isArcived == false).Where(x=>x.DueBackDate <= DateTime.Now.AddDays(7).Date).ToList());
            }
        }

        public ObservableCollection<Fine> GetFines(int id)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var obj = _db.Users.Include(x=>x.Fines).SingleOrDefault(x => x.Id == id);

                return new ObservableCollection<Fine>(obj.Fines.Where(x=>x.IsArchived == false).ToList());
            }
        }

    }
}