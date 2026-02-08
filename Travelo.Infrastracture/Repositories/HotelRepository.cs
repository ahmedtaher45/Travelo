using Microsoft.EntityFrameworkCore;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Review;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository (ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync (PaginationRequest request)
        {
            try
            {
                var query = _context.Hotels
                    .Where(h => h.IsFeatured)
                    .OrderByDescending(h => h.Rating)
                    .ThenByDescending(h => h.ReviewsCount)
                    .Skip((request.PageNumber-1)*request.PageSize)
                    .Take(request.PageSize)
                    .Select(h => new HotelCardDto
                    {
                        Id=h.Id,
                        Name=h.Name,
                        Location=(h.City!=null ? h.City.Name : h.Address)+", "+h.Country,
                        Price=h.PricePerNight,
                        Rating=h.Rating,
                        ImageUrl=h.ImageUrl,
                        ReviewsCount=h.ReviewsCount
                    });

                var data = await query.ToListAsync();

                return GenericResponse<IEnumerable<HotelCardDto>>.SuccessResponse(data);
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<HotelCardDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync (int id)
        {
            try
            {
                var hotelEntity = await _context.Hotels
                    .Include(h => h.City)
                    .Include(h => h.Rooms)
                    .Include(h => h.Reviews!).ThenInclude(r => r.User!)
                    .Include(h => h.ThingsToDo)
                    .FirstOrDefaultAsync(h => h.Id==id);

                if (hotelEntity==null)
                {
                    return GenericResponse<HotelDetailsDto>.FailureResponse("Hotel not found");
                }

                var similarHotels = await _context.Hotels
                    .Where(h => h.CityId==hotelEntity.CityId&&h.Id!=id)
                    .Take(4)
                    .Select(h => new HotelCardDto
                    {
                        Id=h.Id,
                        Name=h.Name,
                        Location=(h.City!=null ? h.City.Name : h.Address)+", "+h.Country,
                        Price=h.PricePerNight,
                        Rating=h.Rating,
                        ImageUrl=h.ImageUrl,
                        ReviewsCount=h.ReviewsCount
                    })
                    .ToListAsync();

                var reviewsData = new HotelReviewDto
                {
                    HotelId=hotelEntity.Id,
                    AvgOverallRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.OverallRating) : 0,
                    AvgCleanlinessRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.CleanlinessRating)??0 : 0,
                    AvgLocationRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.LocationRating)??0 : 0,
                    AvgValueRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.ValueRating)??0 : 0,
                    AvgCommunicationRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.CommunicationRating)??0 : 0,
                    AvgAmenityRate=hotelEntity.Reviews.Any() ? hotelEntity.Reviews.Average(r => r.AmenityRating)??0 : 0,

                    Reviews=hotelEntity.Reviews.OrderByDescending(r => r.CreatedOn).Take(3).Select(r => new ReviewDto
                    {
                        Id=r.Id,
                        UserId=r.UserId,
                        UserName=r.User!=null ? r.User.UserName : "Guest",
                        HotelId=r.HotelId,
                        HotelName=hotelEntity.Name,
                        OverallRate=r.OverallRating,
                        CleanlinessRate=r.CleanlinessRating,
                        LocationRate=r.LocationRating,
                        ValueRate=r.ValueRating,
                        AmenityRate=r.AmenityRating,
                        CommunicationRate=r.CommunicationRating,

                        Comment=r.Comment,
                        CreatedAt=r.CreatedOn??DateTime.UtcNow,
                    }).ToList()
                };

                var hotelDetailsDto = new HotelDetailsDto
                {
                    Id=hotelEntity.Id,
                    Name=hotelEntity.Name,
                    Location=(hotelEntity.City!=null ? hotelEntity.City.Name : hotelEntity.Address)+", "+hotelEntity.Country,
                    Description=hotelEntity.Description,
                    PricePerNight=hotelEntity.PricePerNight,
                    Rating=hotelEntity.Rating,
                    ReviewsCount=hotelEntity.ReviewsCount,
                    ImageUrl=hotelEntity.ImageUrl,
                    Latitude=hotelEntity.Latitude,
                    Longitude=hotelEntity.Longitude,
                    Gallery=new List<string> { hotelEntity.ImageUrl, "https://placehold.co/600x400?text=Room", "https://placehold.co/600x400?text=Pool" },
                    Amenities=new List<string> { "Wifi", "Pool", "Spa", "Parking" },
                    Rooms=hotelEntity.Rooms.Select(r => new RoomDto
                    {
                        Id=r.Id,
                        Type=r.Type,
                        Price=r.PricePerNight,
                        Capacity=r.Capacity,
                        View=r.View,
                        ImageUrl=r.ImageUrl,
                        BedType=r.BedType,
                        Size=r.Size,
                        IsAvailable=r.IsAvailable,
                        RoomAmenities=new List<string> { "Breakfast", "Free Wifi", "No Smoking", "Air Conditioner" }
                    }).ToList(),
                    Policies=new HotelPolicyDto(),
                    ThingsToDo=hotelEntity.ThingsToDo.Select(t => new ThingToDoDto
                    {
                        Id=t.Id,
                        Title=t.Title,
                        Category=t.Category,
                        Distance=t.Distance,
                        Price=t.Price,
                        OldPrice=t.OldPrice,
                        ImageUrl=t.ImageUrl
                    }).ToList(),
                    SimilarHotels=similarHotels,
                    ReviewsData=reviewsData
                };

                return GenericResponse<HotelDetailsDto>.SuccessResponse(hotelDetailsDto, "Success");

            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException!=null ? ex.InnerException.Message : ex.Message;
                return GenericResponse<HotelDetailsDto>.FailureResponse($"Error: {errorMessage}");
            }
        }



        public async Task<GenericResponse<IEnumerable<RoomDto>>> GetRoomsByHotelIdAsync (int hotelId)
        {
            try
            {
                var hotelExists = await _context.Hotels.AnyAsync(h => h.Id==hotelId);
                if (!hotelExists)
                {
                    return GenericResponse<IEnumerable<RoomDto>>.FailureResponse("Hotel not found");
                }

                var rooms = await _context.Rooms
                    .Where(r => r.HotelId==hotelId)
                    .Select(r => new RoomDto
                    {
                        Id=r.Id,
                        Type=r.Type,
                        Price=r.PricePerNight,
                        Capacity=r.Capacity,
                        View=r.View,
                        ImageUrl=r.ImageUrl,
                        BedType=r.BedType,
                        Size=r.Size,
                        IsAvailable=r.IsAvailable,

                        RoomAmenities=new List<string> { "Breakfast", "Free Wifi", "AC" }
                    })
                    .ToListAsync();

                return GenericResponse<IEnumerable<RoomDto>>.SuccessResponse(rooms, "Rooms retrieved successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<RoomDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }




        public async Task<GenericResponse<IEnumerable<ThingToDoDto>>> GetThingsToDoByHotelIdAsync (int hotelId)
        {
            try
            {
                var hotelExists = await _context.Hotels.AnyAsync(h => h.Id==hotelId);
                if (!hotelExists)
                {
                    return GenericResponse<IEnumerable<ThingToDoDto>>.FailureResponse("Hotel not found");
                }

                var things = await _context.ThingsToDo
                    .Where(t => t.HotelId==hotelId)
                    .Select(t => new ThingToDoDto
                    {
                        Id=t.Id,
                        Title=t.Title,
                        Category=t.Category,
                        Distance=t.Distance,
                        Price=t.Price,
                        OldPrice=t.OldPrice,
                        ImageUrl=t.ImageUrl
                    })
                    .ToListAsync();

                return GenericResponse<IEnumerable<ThingToDoDto>>.SuccessResponse(things, "Things to do retrieved successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<ThingToDoDto>>.FailureResponse($"Error: {ex.Message}");
            }

        }


        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> GetSimilarHotelsAsync (int hotelId)
        {
            try
            {

                var currentHotel = await _context.Hotels.FindAsync(hotelId);

                if (currentHotel==null)
                {
                    return GenericResponse<IEnumerable<HotelCardDto>>.FailureResponse("Hotel not found");
                }

                var similarHotels = await _context.Hotels
                    .Where(h => h.CityId==currentHotel.CityId&&h.Id!=hotelId)
                    .Take(4)
                    .Select(h => new HotelCardDto
                    {
                        Id=h.Id,
                        Name=h.Name,
                        Location=(h.City!=null ? h.City.Name : h.Address)+", "+h.Country,
                        Price=h.PricePerNight,
                        Rating=h.Rating,
                        ImageUrl=h.ImageUrl,
                        ReviewsCount=h.ReviewsCount
                    })
                    .ToListAsync();

                return GenericResponse<IEnumerable<HotelCardDto>>.SuccessResponse(similarHotels, "Similar hotels retrieved successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<HotelCardDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> GetAllHotelsAsync(PaginationRequest request)
        {
            try
            {
                var query = _context.Hotels
                    .AsNoTracking() 
                    .OrderByDescending(h => h.Rating)
                    .ThenByDescending(h => h.ReviewsCount)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(h => new HotelCardDto
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Location = (h.City != null ? h.City.Name : h.Address) + ", " + h.Country,
                        Price = h.PricePerNight,
                        Rating = h.Rating,
                        ImageUrl = h.ImageUrl,
                        ReviewsCount = h.ReviewsCount
                    });

                var data = await query.ToListAsync();

                return GenericResponse<IEnumerable<HotelCardDto>>.SuccessResponse(data, "All hotels retrieved successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<HotelCardDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse<string>> UpdateHotelAsync(int id, UpdateHotelDto dto, string currentUserId, bool isAdmin)

        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(id);
                if (hotel == null)
                    return GenericResponse<string>.FailureResponse("Hotel not found");

                if (!isAdmin && hotel.UserId != currentUserId)
                {
                    return GenericResponse<string>.FailureResponse("You are not authorized to update this hotel.");
                }


                hotel.Name = dto.Name;
                hotel.Description = dto.Description;
                hotel.PricePerNight = dto.PricePerNight;
                hotel.Address = dto.Address;
                hotel.ImageUrl = dto.ImageUrl;
                hotel.IsFeatured = dto.IsFeatured;


                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();

                return GenericResponse<string>.SuccessResponse("Hotel updated successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<string>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse<string>> DeleteHotelAsync(int id)
        {
            try
            {
                var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.ThingsToDo)
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.Id == id);

                if (hotel == null)
                {
                    return GenericResponse<string>.FailureResponse("Hotel not found");
                }

                if (hotel.Rooms != null) _context.Rooms.RemoveRange(hotel.Rooms);
                if (hotel.ThingsToDo != null) _context.ThingsToDo.RemoveRange(hotel.ThingsToDo);
                if (hotel.Reviews != null) _context.Reviews.RemoveRange(hotel.Reviews);

                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();

                return GenericResponse<string>.SuccessResponse("Hotel and all related data deleted successfully");
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return GenericResponse<string>.FailureResponse($"Error: {ex.Message}");
            }
        }

    }
}
