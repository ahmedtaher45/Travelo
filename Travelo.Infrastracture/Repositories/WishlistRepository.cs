using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        private readonly ApplicationDbContext context;

        public WishlistRepository (ApplicationDbContext _context) : base(_context)
        {
            context=_context;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistsWithItemsAndHotelsAsync (Expression<Func<Wishlist, bool>> predicate)
        {
            IQueryable<Wishlist> query = context.Wishlists
                .Include(w => w.Items)
                    .ThenInclude(i => i.Hotel);

            if (predicate!=null)
                query=query.Where(predicate);

            return await query.ToListAsync();
        }

    }
}
