using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;
using Travelo.Application.DTOs.Menu;

namespace Travelo.Application.UseCases.Menu
{
    public class AddItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public AddItemUseCase(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<GenericResponse<string>> ExecuteAsync(AddItemDTO dto)
        {
            var category = await _unitOfWork
                .Repository<MenuCategory>()
                .GetById(dto.CategoryId);

            if (category == null || category.IsDeleted)
            {
                return GenericResponse<string>
                    .FailureResponse("Category not found.");
            }

            
            var imageNames = new List<string>();

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    var fileName = await _fileService
                        .UploadFileAsync(image, "MenuItems");

                    if (fileName != null)
                        imageNames.Add(fileName);
                }
            }
            var item = new MenuItem
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Images = imageNames,
                Calories = dto.Calories,
                PrepTime = dto.PrepTime,
                Stock = dto.Stock,
                Ingredients = dto.Ingredients ?? new List<string>(),
                CreatedOn = DateTime.UtcNow
                
            };


            await _unitOfWork.Repository<MenuItem>().Add(item);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item added successfully.");
        }
    }
}
