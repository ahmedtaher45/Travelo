using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Menu
{
    public class GetMenuUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMenuUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericResponse<List<CategoryDTO>>> ExecuteAsync(int restaurantId, string baseUrl = "")
        {
            var restaurant = await _unitOfWork.Repository<Domain.Models.Entities.Restaurant>()
                .GetById(restaurantId);
            if (restaurant == null || restaurant.IsDeleted)
            {
                return GenericResponse<List<CategoryDTO>>.FailureResponse("Restaurant not found.");
            }

            var menu = await _unitOfWork.Menu.GetMenu(restaurantId);
            if (menu == null)
            {
                return GenericResponse<List<CategoryDTO>>.FailureResponse("Menu not found.");
            }
            var returnMenu = menu.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = string.IsNullOrEmpty(c.Image) ? null : $"{baseUrl}MenuCategories/{c.Image}",

                Items = c.items?.Where(i => !i.IsDeleted).Select(i => new ItemDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Description = i.Description,
                    Calories = i.Calories,
                    Stock = i.Stock,
                    Images = i.Images?.Select(img => string.IsNullOrEmpty(img) ? null : $"{baseUrl}MenuItems/{img}").ToList(),
                    PrepTime = i.PrepTime,
                    Ingredients = i.Ingredients

                }).ToList() ?? new List<ItemDTO>()
            }).ToList();

            return GenericResponse<List<CategoryDTO>>
                .SuccessResponse(returnMenu);
        }
    }
}
