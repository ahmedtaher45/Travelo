using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(
            int?pageNum = null,
            int? pageSize = null,
            Func<IQueryable<T>,IQueryable<T>>? include = null);
        public Task<T?> GetById(
            int id,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        public Task Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
