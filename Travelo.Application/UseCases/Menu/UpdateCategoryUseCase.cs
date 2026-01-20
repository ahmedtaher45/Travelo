using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class UpdateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateCategoryUseCase(IUnitOfWork unitOfWork ,IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public async Task<GenericResponse<string>> ExecuteAsync(UpdateCategoryDTO dto)
        {
            var category = await _unitOfWork.Repository<MenuCategory>().GetById(dto.CategoryId);

            if (category == null || category.IsDeleted)
            {
                return GenericResponse<string>.FailureResponse("Category not found.");
            }

            
            if (!string.IsNullOrEmpty(dto.Name))
                category.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Description))
                category.Description = dto.Description;

            
            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(category.Image))
                {
                    await _fileService.DeleteFileAsync(category.Image, "MenuCategories");
                }

                var fileName = await _fileService.UploadFileAsync(dto.Image, "MenuCategories");
                category.Image = fileName;
            }

            category.ModifiedOn = DateTime.UtcNow;

           
            _unitOfWork.Repository<MenuCategory>().Update(category);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Category updated successfully.");
        }

    }
}
