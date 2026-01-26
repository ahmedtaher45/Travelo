using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task AddOrderItemsRangeAsync (List<OrderItem> orderItems);
    }
}
