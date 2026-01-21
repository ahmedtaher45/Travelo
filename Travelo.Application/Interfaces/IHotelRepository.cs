using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {

        Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync (PaginationRequest request);
        Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync (int id);



    }
}
