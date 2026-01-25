using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Review;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
<<<<<<< HEAD

        Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync (PaginationRequest request);
        Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync (int id);


=======
       
        Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync(PaginationRequest request);
        Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync(int id);
        Task<GenericResponse<IEnumerable<RoomDto>>> GetRoomsByHotelIdAsync(int hotelId);
        Task<GenericResponse<IEnumerable<ThingToDoDto>>> GetThingsToDoByHotelIdAsync(int hotelId);
        Task<GenericResponse<IEnumerable<HotelCardDto>>> GetSimilarHotelsAsync(int hotelId);
>>>>>>> origin/Add-Featured-Hotels

    }
}
