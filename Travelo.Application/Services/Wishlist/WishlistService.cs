using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Wishlist;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Wishlist;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork unitOfWork;

    public WishlistService (IUnitOfWork unitOfWork)
    {
        this.unitOfWork=unitOfWork;
    }

    public async Task<GenericResponse<string>> CreateWishlistAsync (
        string userId,
        WishlistRequestDTO wishlist)
    {
        var newWishlist = new Travelo.Domain.Models.Entities.Wishlist
        {
            Title=wishlist.Title,
            UserId=userId
        };

        await unitOfWork.Wishlists.Add(newWishlist);
        await unitOfWork.SaveChangesAsync();

        return GenericResponse<string>.SuccessResponse(
            newWishlist.Id.ToString(),
            "Wishlist created successfully."
        );
    }

    public async Task<GenericResponse<string>> DeleteWishlistAsync (
        string userId,
        int wishlistId)
    {
        var wishlist = await unitOfWork.Wishlists.GetById(wishlistId);

        if (wishlist==null||wishlist.UserId!=userId)
            return GenericResponse<string>
                .FailureResponse("Wishlist not found or access denied.");

        unitOfWork.Wishlists.Delete(wishlist);
        await unitOfWork.SaveChangesAsync();

        return GenericResponse<string>
            .SuccessResponse(null, "Wishlist deleted successfully.");
    }

    public async Task<GenericResponse<IEnumerable<WishListUpdateDTO>>> GetWishlistAsync (
       string userId)
    {
        var wishlists = await unitOfWork.Wishlists
            .GetManyAsync(w => w.UserId==userId)
            ??Enumerable.Empty<Travelo.Domain.Models.Entities.Wishlist>();

        if (!wishlists.Any())
        {
            return GenericResponse<IEnumerable<WishListUpdateDTO>>
                .FailureResponse("No wishlists found for the user.");
        }

        var wishlistDTOs = wishlists.Select(w => new WishListUpdateDTO
        {
            Id=w.Id,
            Title=w.Title
        });

        return GenericResponse<IEnumerable<WishListUpdateDTO>>
            .SuccessResponse(wishlistDTOs, "Wishlists retrieved successfully.");
    }
    public async Task<GenericResponse<IEnumerable<WishlistDTO>>> GetWishlistWithDetalsAsync (string userId, int wishlistId)
    {
        var wishlists = await unitOfWork.Wishlists
            .GetWishlistsWithItemsAndHotelsAsync(w => w.UserId==userId&&w.Id==wishlistId)
            ??Enumerable.Empty<Travelo.Domain.Models.Entities.Wishlist>();

        if (!wishlists.Any())
        {
            return GenericResponse<IEnumerable<WishlistDTO>>
                .FailureResponse("No wishlists found for the user.");
        }

        var wishlistDTOs = wishlists.Select(w => new WishlistDTO
        {
            Id=w.Id,
            Title=w.Title,
            Items=w.Items
                .Where(i => i.Hotel!=null)
                .Select(i => new WishlistItemDTO
                {
                    Id=i.Id,
                    HotelId=i.HotelId,
                    Name=i.Hotel?.Name??"Unknown Hotel",
                    Location=i.Hotel?.Description??"",
                    Price=i.Hotel?.PricePerNight??0,
                    Rating=i.Hotel?.Rating??0,
                    ImageUrl=i.Hotel?.ImageUrl??"",
                    ReviewsCount=i.Hotel?.ReviewsCount??0
                })
                .ToList()
        });

        return GenericResponse<IEnumerable<WishlistDTO>>
            .SuccessResponse(wishlistDTOs, "Wishlists retrieved successfully.");
    }


    public async Task<GenericResponse<IEnumerable<WishlistDTO>>> GetWishlistWithDetalsAsync (string userId)
    {
        var wishlists = await unitOfWork.Wishlists
            .GetWishlistsWithItemsAndHotelsAsync(w => w.UserId==userId)
            ??Enumerable.Empty<Travelo.Domain.Models.Entities.Wishlist>();

        if (!wishlists.Any())
        {
            return GenericResponse<IEnumerable<WishlistDTO>>
                .FailureResponse("No wishlists found for the user.");
        }

        var wishlistDTOs = wishlists.Select(w => new WishlistDTO
        {
            Id=w.Id,
            Title=w.Title,
            Items=w.Items
                .Where(i => i.Hotel!=null)
                .Select(i => new WishlistItemDTO
                {
                    Id=i.Id,
                    HotelId=i.HotelId,
                    Name=i.Hotel?.Name??"Unknown Hotel",
                    Location=i.Hotel?.Description??"",
                    Price=i.Hotel?.PricePerNight??0,
                    Rating=i.Hotel?.Rating??0,
                    ImageUrl=i.Hotel?.ImageUrl??"",
                    ReviewsCount=i.Hotel?.ReviewsCount??0
                })
                .ToList()
        });

        return GenericResponse<IEnumerable<WishlistDTO>>
            .SuccessResponse(wishlistDTOs, "Wishlists retrieved successfully.");
    }

    public async Task<GenericResponse<string>> UpdateWishlistAsync (
        string userId,
        WishlistRequestDTO wishlist,
        int id)
    {
        var existingWishlist = await unitOfWork.Wishlists.GetById(id);

        if (existingWishlist==null||existingWishlist.UserId!=userId)
            return GenericResponse<string>
                .FailureResponse("Wishlist not found or access denied.");

        existingWishlist.Title=wishlist.Title;

        unitOfWork.Wishlists.Update(existingWishlist);
        await unitOfWork.SaveChangesAsync();

        return GenericResponse<string>
            .SuccessResponse(null, "Wishlist updated successfully.");
    }
}
