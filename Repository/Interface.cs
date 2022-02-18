using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IceSync.Repository
{
    public interface IRepository<T> where T : class
    {

        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Update(T entity);
       bool Delete(T entity);
       bool Delete(int id);
        bool DeleteAll();
    }
}
