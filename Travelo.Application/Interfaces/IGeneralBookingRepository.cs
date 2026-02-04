using System.Linq.Expressions;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IGeneralBookingRepository : IGenericRepository<GeneralBooking>
    {
        Task<IEnumerable<GeneralBooking>> GetManyAsync (Expression<Func<GeneralBooking, bool>> predicate = null, Func<IQueryable<GeneralBooking>, IQueryable<GeneralBooking>> include = null);



    }
}
