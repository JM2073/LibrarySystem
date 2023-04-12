using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Main.XML
{
    public class DbSet<T> where T : class
    {
        private readonly XmlContext _context;
        private readonly List<T> _entities;

        public DbSet(string filePath, XmlContext context)
        {
            _context = context;
            _entities = new List<T>(_context.LoadData<T>());
        }

        public IEnumerable<T> All()
        {
            return _entities;
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _entities.AsQueryable().FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
            _context.SaveData(_entities.ToArray());
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
            _context.SaveData(_entities.ToArray());
        }

    }
}