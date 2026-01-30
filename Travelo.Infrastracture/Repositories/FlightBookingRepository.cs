using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class FlightBookingRepository : GenericRepository<Travelo.Domain.Models.Entities.FlightBooking>, Travelo.Application.Interfaces.IFlightBookingRepository
    {
        public FlightBookingRepository (ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
