using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class DeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase (IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public async Task<GenericResponse<string>> ExecuteAsync (int categoryId, string user)
        {

            var category = await _unitOfWork.Repository<MenuCategory>().GetById(categoryId);

            if (category==null||category.IsDeleted)
            {
                return GenericResponse<string>.FailureResponse("Category not found.");
            }
            var restaurant = await _unitOfWork.Restaurant.GetById(category.RestaurantId);
            if (restaurant==null||restaurant.UserId!=user)
            {
                return GenericResponse<string>.FailureResponse("Unauthorized: You do not own this restaurant.");
            }

            category.IsDeleted=true;


            if (category.items!=null&&category.items.Any())
            {
                foreach (var item in category.items)
                {
                    item.IsDeleted=true;

                    _unitOfWork.Repository<MenuItem>().Update(item);
                }
            }

            _unitOfWork.Repository<MenuCategory>().Update(category);

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Category deleted successfully.");
        }

    }
}
