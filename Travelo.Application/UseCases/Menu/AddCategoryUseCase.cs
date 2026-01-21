using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class AddCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
<<<<<<< HEAD
        private readonly IFileServices _fileService;

        public AddCategoryUseCase(IUnitOfWork unitOfWork ,IFileServices fileService)
=======
        private readonly IFileService _fileService;

        public AddCategoryUseCase(IUnitOfWork unitOfWork ,IFileService fileService)
>>>>>>> origin/main
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<GenericResponse<string>> ExecuteAsync(AddCategoryDTO dto)
        {
           
            var restaurant = await _unitOfWork
                .Repository<Domain.Models.Entities.Restaurant>()
                .GetById(dto.RestaurantId);

            if (restaurant == null)
            {
                return GenericResponse<string>.FailureResponse("Restaurant not found.");
            }

            
            string? fileName = null;
            if (dto.Image != null)
            {
                fileName = await _fileService.UploadFileAsync(dto.Image, "MenuCategories");
            }

            
            var category = new MenuCategory
            {
                RestaurantId = dto.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Image = fileName,       
                items = new List<MenuItem>(),
                CreatedOn = DateTime.UtcNow
            };

            
            await _unitOfWork.Repository<MenuCategory>().Add(category);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Category added successfully.");
        }





    }
}
