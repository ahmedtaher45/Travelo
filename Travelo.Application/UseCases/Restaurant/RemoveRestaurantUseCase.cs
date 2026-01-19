using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Restaurant
{
    public class RemoveRestaurantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRestaurantUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> RemoveRestaurant(int id)
        {
            var restaurant = await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().GetById(id);
            if (restaurant == null) {
                return new GenericResponse<string>{Success = false, Message = "Restaurant not found"};
            }
              _unitOfWork.Repository<Domain.Models.Entities.Restaurant>().Delete(restaurant);
            await _unitOfWork.SaveChangesAsync();

            return new GenericResponse<string>{Success = true, Message = "Restaurant deleted successfully"};
        }
    }
}
