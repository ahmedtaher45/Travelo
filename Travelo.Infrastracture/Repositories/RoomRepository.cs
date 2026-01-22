using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class RoomRepository : GenericRepository<Domain.Models.Entities.Room>, Application.Interfaces.IRoomRepository
    {
        public RoomRepository (ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
