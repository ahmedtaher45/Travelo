using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class RestaurantRepository : GenericRepository<Domain.Models.Entities.Restaurant>, Application.Interfaces.IRestaurantRepository
    {
        public RestaurantRepository (ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
