using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class RoomRepository : GenericRepository<Domain.Models.Entities.Room>, Application.Interfaces.IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository (ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetAvailableRoomsAsync(int hotelId,DateTime checkIn,  DateTime checkOut  )
        {
            var roomsQuery = _context.Rooms
                .Where(r => r.HotelId == hotelId && r.IsAvailable);

            var bookedRoomIds = _context.RoomBookings
                .Where(b =>
                    checkIn < b.CheckOutDate &&
                    checkOut > b.CheckInDate
                )
                .Select(b => b.RoomId)
                .Distinct();

            return await roomsQuery
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .ToListAsync();
        }
    }
}
