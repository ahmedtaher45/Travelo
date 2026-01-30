using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Menu;
using Travelo.Application.UseCases.Menu;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AddCategoryUseCase _addCategoryUseCase;
        private readonly GetMenuUseCase _getMenuUseCase;
        private readonly AddItemUseCase _addItemUseCase;
        private readonly DeleteItemUseCase _deleteItemUseCase;
        private readonly UpdateItemUseCase _updateItemUseCase;
        private readonly DeleteCategoryUseCase _deleteCategoryUseCase;
        private readonly UpdateCategoryUseCase _updateCategoryUseCase;
        private readonly GetItemUseCase _getItemUseCase;

        public  MenuController(AddCategoryUseCase addCategoryUseCase, 
                                GetMenuUseCase getMenuUseCase , 
                                AddItemUseCase addItemUseCase , 
                                DeleteItemUseCase deleteItemUseCase,
                                UpdateItemUseCase updateItemUseCase,
                                DeleteCategoryUseCase deleteCategoryUseCase,
                                UpdateCategoryUseCase updateCategoryUseCase,
                                GetItemUseCase getItemUseCase)
        {
            _addCategoryUseCase = addCategoryUseCase;
            _getMenuUseCase = getMenuUseCase;
           _addItemUseCase = addItemUseCase;
            _deleteItemUseCase = deleteItemUseCase;
            _updateItemUseCase = updateItemUseCase;
            _deleteCategoryUseCase = deleteCategoryUseCase;
            _updateCategoryUseCase = updateCategoryUseCase;
            _getItemUseCase = getItemUseCase;
        }

        [HttpPost("menu/{restaurantId}")]
        public async Task<IActionResult> GetMenu([FromRoute]int restaurantId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            var result = await _getMenuUseCase.ExecuteAsync(restaurantId,baseUrl);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("category")]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryDTO dto)
        {
            var result = await _addCategoryUseCase.ExecuteAsync(dto);

            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("item")]
        public async Task<IActionResult> AddItem([FromForm] AddItemDTO dto)
        {
            var result = await _addItemUseCase.ExecuteAsync(dto);
            return !result.Success ? BadRequest(result) : Ok(result);

        }
        [HttpDelete("item/{itemId}")]
        public async Task<IActionResult> DeleteItem( int itemId)
        {
            var result = await _deleteItemUseCase.ExecuteAsync(itemId);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPut("item")]
        public async Task<IActionResult> UpdateItem([FromForm] UpdateItemDTO dto)
        {
            var result = await _updateItemUseCase.ExecuteAsync(dto);
            return !result.Success ? BadRequest(result) : Ok(result);
        }

        [HttpPut("category")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryDTO dto)
        {
            var result = await _updateCategoryUseCase.ExecuteAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            var result = await _deleteCategoryUseCase.ExecuteAsync(categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetItem([FromRoute] int itemId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            var result = await _getItemUseCase.ExecuteAsync(itemId, baseUrl);

            return result.Success ? Ok(result) : NotFound(result);
        }






    }
}
