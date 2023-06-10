using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // Get by Id
        T Get(int id);
        // Get all
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
        );
        // Get first or default
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
        );
        // Add
        void Add(T entity);
        // Remove by Id
        void Remove(int id);
        // Remove by entity
        void Remove(T entity);
    }
}