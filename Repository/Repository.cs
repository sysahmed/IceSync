using IceSync.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IceSync.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MyDbContext _context;
        private readonly DbSet<T> _dbSet;


        public Repository(MyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).SingleOrDefault();
        }

        public T Add(T entity)
        {
           var result= _dbSet.Add(entity).Entity;
            return result;
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public bool Delete(int id)
        {
            var result=_dbSet.Find(id);
            _dbSet.Remove(result);
            return true;
        }

        public T GetById(int id)
        {
           return  _dbSet.Find(id);

        }

        public bool DeleteAll()
        {
            List<T> result = _dbSet.ToList();
            _dbSet.RemoveRange(result);
            return true;
        }
    }
 
}
