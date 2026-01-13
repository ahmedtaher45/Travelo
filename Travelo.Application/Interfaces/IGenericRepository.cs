using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
         Task<IEnumerable<T>> GetAll(
            int?pageNum = null,
            int? pageSize = null,
            Func<IQueryable<T>,IQueryable<T>>? include = null);
         Task<T?> GetById(
            int id,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<List<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
         Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
