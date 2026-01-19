using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Restaurant
{
    public class AddRestaurantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddRestaurantUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> AddRestaurant(int cityId,AddRestaurantDto dto)
        {
            var city = await _unitOfWork.Repository<Domain.Models.Entities.City>().GetById(cityId);
            if (city == null)
            {
                return new GenericResponse<string> { Data = "City not found" };
            }
            var restaurant = new Domain.Models.Entities.Restaurant
            {
                Name = dto.Name,
                Description = dto.Description,
                CityId = city.Id,
                CreatedOn = DateTime.UtcNow
            };
            
            await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().Add(restaurant);
            await _unitOfWork.SaveChangesAsync();
            return new GenericResponse<string> { Data = "Restaurant added successfully" };
        }
    }
}
