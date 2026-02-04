using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class GeneralBookingRepository : GenericRepository<GeneralBooking>, IGeneralBookingRepository
    {
        private readonly ApplicationDbContext context;

        public GeneralBookingRepository (ApplicationDbContext _context) : base(_context)
        {
            context=_context;
        }

        public async Task<IEnumerable<GeneralBooking>> GetManyAsync (
        Expression<Func<GeneralBooking, bool>> predicate = null,
         Func<IQueryable<GeneralBooking>, IQueryable<GeneralBooking>> include = null)
        {
            IQueryable<GeneralBooking> query = context.GeneralBookings;

            if (include!=null)
            {
                query=include(query);
            }

            if (predicate!=null)
            {
                query=query.Where(predicate);
            }

            return await query.ToListAsync();
        }
    }
}
