using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.UseCases.Restaurant;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly AddRestaurantUseCase _addRestaurantUseCase;
        private readonly GetRestaurantUseCase _getRestaurantUseCase;
        private readonly UpdateRestaurantUseCase _updateRestaurantUseCase;
        private readonly RemoveRestaurantUseCase _removeRestaurantUseCase;
        private readonly GetAllRestaurantsUseCase _getAllRestaurantsUseCase;
        public RestaurantController (AddRestaurantUseCase addRestaurantUseCase, GetRestaurantUseCase getRestaurantUseCase, UpdateRestaurantUseCase updateRestaurantUseCase, RemoveRestaurantUseCase removeRestaurantUseCase, GetAllRestaurantsUseCase getAllRestaurantsUseCase)
        {
            _addRestaurantUseCase = addRestaurantUseCase;
            _getRestaurantUseCase = getRestaurantUseCase;
            _updateRestaurantUseCase = updateRestaurantUseCase;
            _removeRestaurantUseCase = removeRestaurantUseCase;
            _getAllRestaurantsUseCase = getAllRestaurantsUseCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant (int id)
        {
            try
            {
                var result = await _getRestaurantUseCase.GetRestaurant(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRestaurant (int cityId, AddRestaurantDto dto)
        {
            try
            {
                var res = await _addRestaurantUseCase.AddRestaurant(dto);
                return Created("", res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRestaurant ([FromRoute] int id, [FromBody] AddRestaurantDto dto)
        {
            try
            {
                var res = await _updateRestaurantUseCase.UpdateRestaurant(id, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRestaurant ([FromRoute] int id)
        {
            try
            {
                await _removeRestaurantUseCase.RemoveRestaurant(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet] // GET: api/Restaurant
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var result = await _getAllRestaurantsUseCase.ExecuteAsync();
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
