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
    public class GetAllRestaurantsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllRestaurantsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<RestaurantDto>>> ExecuteAsync()
        {
            try
            {
                var restaurants = await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().GetAll();            
                var data = restaurants.Select(r => new RestaurantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                    
                });
                return GenericResponse<IEnumerable<RestaurantDto>>.SuccessResponse(data, "Restaurants retrieved successfully");
            }
            catch (Exception ex)
            {
                return GenericResponse<IEnumerable<RestaurantDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
