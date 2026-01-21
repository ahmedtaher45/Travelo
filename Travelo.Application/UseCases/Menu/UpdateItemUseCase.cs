using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Menu
{
    public class UpdateItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileService;
        public UpdateItemUseCase (IUnitOfWork unitOfWork, IFileServices fileService)
        {
            _unitOfWork=unitOfWork;
            _fileService=fileService;
        }
        public async Task<GenericResponse<string>> ExecuteAsync (UpdateItemDTO dto)
        {
            var item = await _unitOfWork.Repository<MenuItem>()
                .GetById(dto.ItemId);

            if (item==null||item.IsDeleted)
                return GenericResponse<string>
                    .FailureResponse("Item not found.");

            if (dto.Name!=null)
                item.Name=dto.Name;

            if (dto.Price.HasValue)
                item.Price=dto.Price.Value;

            if (dto.Description!=null)
                item.Description=dto.Description;

            if (dto.Calories.HasValue)
                item.Calories=dto.Calories.Value;

            if (dto.PrepTime.HasValue)
                item.PrepTime=dto.PrepTime.Value;

            if (dto.Stock.HasValue)
                item.Stock=dto.Stock.Value;

            if (dto.Ingredients!=null)
                item.Ingredients=dto.Ingredients.ToList();

            if (dto.RemovedImages!=null&&item.Images.Any())
            {
                foreach (var image in dto.RemovedImages)
                {
                    if (item.Images.Contains(image))
                    {
                        await _fileService
                            .DeleteFileAsync(image, "MenuItems");

                        item.Images.Remove(image);
                    }
                }
            }

            if (dto.NewImages!=null)
            {
                foreach (var image in dto.NewImages)
                {
                    var fileName = await _fileService
                        .UploadFileAsync(image, "MenuItems");

                    if (fileName!=null)
                    {
                        item.Images.Add(fileName);
                    }
                }
            }

            item.ModifiedOn=DateTime.Now;

            _unitOfWork.Repository<MenuItem>().Update(item);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>
                .SuccessResponse("Item updated successfully.");
        }

    }
}
