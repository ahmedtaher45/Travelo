using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.WishlistItem;
using Travelo.Application.Interfaces;

namespace Travelo.Application.Services.WishlistItem
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly IUnitOfWork unitOfWork;

        public WishlistItemService (IUnitOfWork unitOfWork)
        {
            this.unitOfWork=unitOfWork;
        }

        public async Task<GenericResponse<string>> AddWishListItem (
            string userId,
            WishlistItemReqDTO reqDTO)
        {
            var wishlist = await unitOfWork.Wishlists.GetById(reqDTO.ListId);

            if (wishlist==null||wishlist.UserId!=userId)
            {
                return GenericResponse<string>
                    .FailureResponse("Wishlist not found or access denied.");
            }

            var hotel = await unitOfWork.Hotels.GetById(reqDTO.HotelId);
            if (hotel==null)
            {
                return GenericResponse<string>
                    .FailureResponse("Hotel not found.");
            }
            var existingItem = await unitOfWork.WishlistItems
                .GetManyAsync(i =>
                    i.WishlistId==reqDTO.ListId&&
                    i.HotelId==reqDTO.HotelId);

            if (existingItem.Any())
            {
                return GenericResponse<string>
                    .FailureResponse("Hotel already exists in this wishlist.");
            }

            var wishlistItem = new Travelo.Domain.Models.Entities.WishlistItem
            {
                WishlistId=reqDTO.ListId,
                HotelId=reqDTO.HotelId
            };

            await unitOfWork.WishlistItems.Add(wishlistItem);
            await unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse(
                    wishlistItem.Id.ToString(),
                    "Wishlist item added successfully."
                );
        }

        public async Task<GenericResponse<string>> DeleteWishListItem (
            string userId,
            int itemId)
        {
            var item = await unitOfWork.WishlistItems.GetById(itemId);

            if (item==null)
            {
                return GenericResponse<string>
                    .FailureResponse("Wishlist item not found.");
            }

            var wishlist = await unitOfWork.Wishlists.GetById(item.WishlistId);

            if (wishlist==null||wishlist.UserId!=userId)
            {
                return GenericResponse<string>
                    .FailureResponse("Access denied.");
            }

            unitOfWork.WishlistItems.Delete(item);
            await unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse(null, "Item deleted successfully.");
        }
    }
}
