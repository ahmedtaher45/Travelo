using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Restaurant
{
    public class AddRestaurantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddRestaurantUseCase (IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<GenericResponse<string>> AddRestaurant (AddRestaurantDto dto)
        {
            var city = await _unitOfWork.Repository<Domain.Models.Entities.City>().GetById(dto.CityId);
            return city==null ? new GenericResponse<string> { Data="City not found" } : await _unitOfWork.Auth.AddRestaurant(dto.CityId, dto);
        }
    }
}
