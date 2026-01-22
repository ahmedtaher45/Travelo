using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class RoomBookingRepository : GenericRepository<Travelo.Domain.Models.Entities.RoomBooking>, Travelo.Application.Interfaces.IRoomBookingRepository
    {
        public RoomBookingRepository (ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
