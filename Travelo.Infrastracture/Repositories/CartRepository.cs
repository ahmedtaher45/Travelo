using Microsoft.EntityFrameworkCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository (ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task ClearCart (string userId)
        {
            var cartItem = _context.Carts.Where(c => c.UserId==userId);
            _context.Carts.RemoveRange(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartByUserId (string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId==userId&&!c.IsDeleted);
        }
    }
}
