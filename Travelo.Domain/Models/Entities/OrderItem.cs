using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
