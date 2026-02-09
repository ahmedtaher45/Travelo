using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Restaurant
{
    public class UpdateRestaurantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRestaurantUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> UpdateRestaurant(int id, RestaurantDto request, string currentUserId, List<string> userRoles)
        {

            var existingRestaurant = await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().GetById(id);

            if (existingRestaurant == null)
            {
                return new GenericResponse<string> { Message = "Restaurant not found", Success = false };
            }
            bool isAdmin = userRoles.Contains("Admin");
            bool isOwner = existingRestaurant.UserId == currentUserId;
            if (!isAdmin && !isOwner)
            {
                return new GenericResponse<string> { Message = "Unauthorized action", Success = false };
            }
      
            existingRestaurant.Name = request.Name;
            existingRestaurant.Description = request.Description;
            _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().Update(existingRestaurant);
            await _unitOfWork.SaveChangesAsync();

            return new GenericResponse<string> { Message = "Restaurant updated successfully", Success = true };

        }

    }
}
