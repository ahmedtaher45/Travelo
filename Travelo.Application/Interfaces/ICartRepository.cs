using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface ICartRepository
    {
        public Task<Cart?> GetCartByUserId (string userId);
        public Task ClearCart (string userId);
    }
}
