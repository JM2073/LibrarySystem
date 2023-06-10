using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using LibrarySystem.Domain.Models;
using LibrarySystem.EntityFramework;
using LibrarySystem.WPF.Stores;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.WPF.Servies
{
    public class FineService
    {
        private readonly AccountStore _accountStore;
        private readonly LogService _logService;
        private readonly XDocument _userDoc;
        private static readonly DateTime PAY_BY_DATE = DateTime.Now.AddDays(7);
        private const decimal TIMES_VALUE_BY = (decimal)0.15;

        private LibraryDBContextFactory _dbContextFactory;

        public FineService(AccountStore accountStore)
        {
            _accountStore = accountStore;
            _dbContextFactory = new LibraryDBContextFactory();
            _logService = new LogService();
        }

        public void AddFine(Fine fine)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                fine.logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "Fine was added agnest the user.",
                });
                _db.Fines.Add(fine);

                _db.SaveChanges();
            }
        }

        public void EditFine(Fine fine)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var fineEntity = _db.Fines.Include(x => x.logs).SingleOrDefault(x => x.Id == fine.Id);

                fineEntity.FineAmount = fine.FineAmount;
                fineEntity.Reason = fine.Reason;
                fineEntity.PayByDate = fine.PayByDate;
                fineEntity.logs.Add(_logService.AddLog("Fine was edited"));
                _db.SaveChanges();
            }
        }


        public void DeleteFine(Fine fine)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var fineEntity = _db.Fines.Include(x => x.logs).SingleOrDefault(x => x.Id == fine.Id);

                fineEntity.IsArchived = true;
                fineEntity.logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "Fine was archived",
                });
                _db.SaveChanges();
            }
        }

        public void PayFine(int fineId)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var fineEntity = _db.Fines.Include(x => x.logs).SingleOrDefault(x => x.Id == fineId);

                fineEntity.IsArchived = true;
                fineEntity.IsPayed = true;
                fineEntity.logs.Add(new Log
                {
                    Date = DateTime.Now,
                    Description = "Fine was payed",
                });
                _db.SaveChanges();
            }
        }

        public List<Fine> GetAllFines()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Fines.Where(x => x.IsArchived == false).ToList();
            }
        }

        public List<Fine> GetUserFines(int userId)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Fines.Where(x => x.UserId == userId).Where(x => x.IsArchived == false).ToList();
            }
        }

        /// <summary>
        ///     this method runs at the start of the application
        ///     it checks if there are any new fines that need issuing, as well as updates any existing fines if there overdue.
        /// </summary>
        public void CheckFines()
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                var users = _db.Users
                    .Include(x => x.Fines).ThenInclude(x=>x.logs)
                    .Include(x => x.Books).ThenInclude(x=>x.Fines).ThenInclude(x=>x.logs)
                    .Include(x => x.Logs)
                    .Where(x => x.isArcived == false)
                    .ToList();

                foreach (var user in users)
                {
                    var userBooks = user.Books.Where(x => x.isArcived == false).ToList();
                    var userFines = user.Fines.Where(x => x.IsArchived == false).Where(x => x.IsPayed == false)
                        .ToList();


                    if (userBooks.Any())
                    {
                        foreach (var userBook in userBooks)
                        {
                            if (userBook.Fines.Where(x => x.IsArchived == false).Where(x => x.IsPayed == false).Any())
                                continue;

                            if (!(userBook.DueBackDate < DateTime.Now))
                                continue;

                            var fineCost = TIMES_VALUE_BY * userBook.BookCost;

                            user.Fines.Add(new Fine
                            {
                                BookId = userBook.Id,
                                FineAmount = fineCost,
                                Reason = "book Late Back",
                                PayByDate = PAY_BY_DATE,
                                IsArchived = false,
                                IsPayed = false,
                                logs = new List<Log>
                                {
                                    _logService.AddLog("fine added to person for late book", bookId: userBook.Id,
                                        userId: user.Id)
                                }
                            });
                        }
                    }

                    if (!userFines.Any())
                        continue;

                    foreach (var userFine in userFines)
                    {
                        if (userFine.PayByDate >= DateTime.Now)
                            continue;

                        var currentFine = userFine.FineAmount;
                        var bookCost = userFine.Book.BookCost;

                        if (bookCost <= currentFine)
                        {
                            userFine.logs.Add(_logService.AddLog("fine not paid, send final warning.",
                                bookId: userFine.BookId, userId: user.Id));
                            userFine.FinalWarningSent = true;
                        }

                        var newFine = currentFine + TIMES_VALUE_BY * bookCost;
                        userFine.FineAmount = newFine;

                        userFine.PayByDate = DateTime.Now.AddDays(7);

                        userFine.logs.Add(_logService.AddLog(
                            "fine not paid, increased fine amount and given them 7 more days.", bookId: userFine.BookId,
                            userId: user.Id));
                    }
                }

                _db.SaveChanges();
            }
        }

        public bool CheckForFine(string isbn, int userId)
        {
            using (var _db = _dbContextFactory.CreateDbContext())
            {
                return _db.Users
                    .Include(x => x.Books)
                    .Include(x => x.Fines).ThenInclude(x=>x.Book)
                    .Single(x => x.Id == userId).Fines.Any(x => x.Book.Isbn == isbn);
            }
        }
    }
}