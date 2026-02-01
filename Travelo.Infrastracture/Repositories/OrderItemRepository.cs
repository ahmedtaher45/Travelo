using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext context;

        public OrderItemRepository (ApplicationDbContext _context) : base(_context)
        {
            context=_context;
        }
        public async Task AddOrderItemsRangeAsync (List<OrderItem> orderItems)
        {
            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
        }
    }
}
