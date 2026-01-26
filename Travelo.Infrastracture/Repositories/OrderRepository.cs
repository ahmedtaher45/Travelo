using Travelo.Application.Interfaces;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class OrderRepository : GenericRepository<Travelo.Domain.Models.Entities.Order>, IOrderRepository
    {
        public OrderRepository (ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
