using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync(PaginationRequest request)
        {
            try
            {
                var query = _context.Hotels               
                    .Where(h => h.IsFeatured)
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

                return GenericResponse<IEnumerable<HotelCardDto>>.SuccessResponse(data);
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<HotelCardDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync(int id)
        {
            try
            {
                var hotelDto = await _context.Hotels
                    .Where(h => h.Id == id)
                    .Select(h => new HotelDetailsDto
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Location = (h.City != null ? h.City.Name : h.Address) + ", " + h.Country,
                        Description = h.Description,
                        PricePerNight = h.PricePerNight,
                        Rating = h.Rating,
                        ReviewsCount = h.ReviewsCount,
                        ImageUrl = h.ImageUrl,
                        Latitude = h.Latitude,
                        Longitude = h.Longitude,
                        Gallery = !string.IsNullOrEmpty(h.ImageUrl) ? new List<string> { h.ImageUrl } : new List<string>(),                      
                        Amenities = new List<string>(),


                        //  Mock Data 
                        //Gallery = new List<string> { h.ImageUrl, "https://placehold.co/600x400?text=Room", "https://placehold.co/600x400?text=Pool" },
                        // Amenities = new List<string> { "Wifi", "Pool", "Spa", "Parking" }

                        Rooms = h.Rooms.Select(r => new RoomDto
                         {
                             Id = r.Id,
                             Type = r.Type,
                             Price = r.PricePerNight,
                             Capacity = r.Capacity,
                             View = r.View,
                             ImageUrl = r.ImageUrl,
                             BedType = r.BedType, 
                             Size = r.Size,
                            IsAvailable = r.IsAvailable,
                            RoomAmenities = new List<string> { "Breakfast", "Free Wifi", "No Smoking", "Air Conditioner" }

                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (hotelDto == null)
                {
                    return GenericResponse<HotelDetailsDto>.FailureResponse("Hotel not found");
                }

                return GenericResponse<HotelDetailsDto>.SuccessResponse(hotelDto, "Hotel details retrieved successfully");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return GenericResponse<HotelDetailsDto>.FailureResponse($"Error: {errorMessage}");
            }
        }

    }
}
