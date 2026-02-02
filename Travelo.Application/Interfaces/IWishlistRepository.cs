using System.Linq.Expressions;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetWishlistsWithItemsAndHotelsAsync (Expression<Func<Wishlist, bool>> predicate);


    }
}
