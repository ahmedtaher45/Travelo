using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Wishlist;

namespace Travelo.Application.Services.Wishlist
{
    public interface IWishlistService
    {
        Task<GenericResponse<IEnumerable<WishListUpdateDTO>>> GetWishlistAsync (string userId);
        Task<GenericResponse<IEnumerable<WishlistDTO>>> GetWishlistWithDetalsAsync (string userId);
        Task<GenericResponse<IEnumerable<WishlistDTO>>> GetWishlistWithDetalsAsync (string userId, int wishlistId);
        Task<GenericResponse<string>> UpdateWishlistAsync (string userId, WishlistRequestDTO wishlist, int id);
        Task<GenericResponse<string>> CreateWishlistAsync (string userId, WishlistRequestDTO wishlist);
        Task<GenericResponse<string>> DeleteWishlistAsync (string userId, int wishlistId);
    }
}
