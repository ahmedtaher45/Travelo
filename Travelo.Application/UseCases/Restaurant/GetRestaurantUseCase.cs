using Microsoft.EntityFrameworkCore;
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
    public class GetRestaurantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRestaurantUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<RestaurantDto>> GetRestaurant(int id)
        {
            var restaurants = await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>()
                .GetById(id);
            if (restaurants == null)
            {
                return new GenericResponse<RestaurantDto>
                {
                    Message = "Restaurant not found",
                    Success = false
                };
            }
            var restaurantsDto = new RestaurantDto
            {
                Id = restaurants.Id,
                Name = restaurants.Name,
                Description = restaurants.Description,
            };
            return new GenericResponse<RestaurantDto> { Data = restaurantsDto, Success = true }; //GenericResponse

        }
    }
}
