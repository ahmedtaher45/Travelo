using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Domain.Models.Entities.Room>
    {
        Task<List<Room>> GetAvailableRoomsAsync(
        int hotelId,
        DateTime checkIn,
        DateTime checkOut );
    }
}
