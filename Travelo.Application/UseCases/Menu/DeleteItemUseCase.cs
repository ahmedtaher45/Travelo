using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class DeleteItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileService;

        public DeleteItemUseCase (IUnitOfWork unitOfWork, IFileServices fileService)
        {
            _unitOfWork=unitOfWork;
            _fileService=fileService;
        }
        public async Task<GenericResponse<string>> ExecuteAsync (int itemId, string user)
        {
            var item = await _unitOfWork
                .Repository<MenuItem>()
                .GetById(itemId);
            var catigory = await _unitOfWork.Repository<MenuCategory>().GetById(itemId);
            var restaurant = await _unitOfWork.Restaurant.GetById(catigory.RestaurantId);

            if (restaurant==null||restaurant.UserId!=user)
            {
                return GenericResponse<string>.FailureResponse("Unauthorized: You do not own this restaurant.");
            }
            if (item==null||item.IsDeleted)
            {
                return GenericResponse<string>
                    .FailureResponse("Item not found.");
            }


            item.IsDeleted=true;
            _unitOfWork.Repository<MenuItem>().Update(item);

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item deleted successfully.");
        }
    }
}
