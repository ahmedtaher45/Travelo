using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Travelo.Application.Interfaces;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<T> _set;

        public GenericRepository (ApplicationDbContext _context)
        {
            context=_context;
            _set=context.Set<T>();
        }
        public virtual async Task<IEnumerable<T>> GetAll (
            int? pageNum = null,
            int? pageSize = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (include is not null)
            {
                query=include(query);
                query=query.AsNoTracking();
            }
            if (pageNum.HasValue&&pageSize.HasValue)
            {
                query=query
                    .Skip((pageNum.Value-1)*pageSize.Value)
                    .Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetById (
            int id,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (include is not null)
            {
                query=include(query);
            }
            return await query
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id")==id);
        }
        public async Task<List<T>> GetManyAsync (Expression<Func<T, bool>> predicate)
        {
            return await _set.Where(predicate).AsNoTracking().ToListAsync();
        }
        public virtual async Task Add (T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public virtual void Update (T entity)
        {
            context.Set<T>().Update(entity);
        }

        public virtual void Delete (T entity)
        {
            context.Set<T>().Remove(entity);

        }

    }
}
