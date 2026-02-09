using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class AddItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileService;

        public AddItemUseCase (
            IUnitOfWork unitOfWork,
            IFileServices fileService)
        {
            _unitOfWork=unitOfWork;
            _fileService=fileService;
        }

        public async Task<GenericResponse<string>> ExecuteAsync (AddItemDTO dto, string user)
        {
            var category = await _unitOfWork
                .Repository<MenuCategory>()
                .GetById(dto.CategoryId);

            if (category==null||category.IsDeleted)
            {
                return GenericResponse<string>
                    .FailureResponse("Category not found.");
            }
            var restaurant = await _unitOfWork.Restaurant.GetById(category.RestaurantId);
            if (restaurant.UserId==user)
            {
                return GenericResponse<string>
                    .FailureResponse("You are not Authrize to ");
            }
            var imageNames = new List<string>();

            if (dto.Images!=null&&dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    var fileName = await _fileService
                        .UploadFileAsync(image, "MenuItems");

                    if (fileName!=null)
                        imageNames.Add(fileName);
                }
            }
            var item = new MenuItem
            {
                CategoryId=dto.CategoryId,
                Name=dto.Name,
                Price=dto.Price,
                Description=dto.Description,
                Images=imageNames,
                Calories=dto.Calories,
                PrepTime=dto.PrepTime,
                Stock=dto.Stock,
                Ingredients=dto.Ingredients??new List<string>(),
                CreatedOn=DateTime.UtcNow
            };

            await _unitOfWork.Repository<MenuItem>().Add(item);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item added successfully.");
        }
    }
}
