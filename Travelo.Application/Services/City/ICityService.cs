using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.City;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Restaurant;

namespace Travelo.Application.Services.City
{
    public interface ICityService
    {
        Task<GenericResponse<IEnumerable<CityResDTO>>> GetAllCitiesAsync (int? pageNum = null,
            int? pageSize = null, string url = "");
        Task<GenericResponse<CityResDTO>> GetCityByIdAsync (int cityId, string url);
        Task<GenericResponse<string>> CreateCity (CityReqDTO cityReq);
        Task<GenericResponse<string>> UpdateCity (int cityId, CityReqDTO cityReq);
        Task<GenericResponse<string>> RemoveCity (int cityId);
        Task<GenericResponse<IEnumerable<RestaurantDto>>> GetCityRestorants (int cityId);
        Task<GenericResponse<IEnumerable<RestaurantDto>>> GetCityTopRestorants (int cityId);
        Task<GenericResponse<IEnumerable<HotelDetailsDto>>> GetCityHotels (int cityId);
    }
}
