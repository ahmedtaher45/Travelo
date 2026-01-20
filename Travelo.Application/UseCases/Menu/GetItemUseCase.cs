using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Menu
{
    public class GetItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetItemUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<ItemDTO>> ExecuteAsync(int itemId, string baseUrl = "")
        {
            
            var item = await _unitOfWork.Repository<Domain.Models.Entities.MenuItem>()
                .GetById(itemId);

            if (item == null || item.IsDeleted)
            {
                return GenericResponse<ItemDTO>.FailureResponse("Item not found.");
            }

            
            var itemDto = new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                Calories = item.Calories,
                PrepTime = item.PrepTime,
                Stock = item.Stock,
                Ingredients = item.Ingredients,
                Images = item.Images?.Select(img => string.IsNullOrEmpty(img) ? null : $"{baseUrl}MenuItems/{img}").ToList()
            };

            return GenericResponse<ItemDTO>.SuccessResponse(itemDto);
        }
    }
}
