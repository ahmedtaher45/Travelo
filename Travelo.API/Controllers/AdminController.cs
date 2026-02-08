using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.UseCases.Auth;
using Travelo.Application.UseCases.Hotels;
using Travelo.Application.UseCases.Restaurant;

namespace Travelo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("add-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin (
            [FromBody] AdminDTO adminDTO,
            [FromServices] AddAdminUseCase addAdminUseCase)
        {
            var result = await addAdminUseCase.ExecuteAsync(adminDTO);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("add-restaurant")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRestaurant (
            [FromBody] AddRestaurantDto addRestaurantDto,
            [FromServices] AddRestaurantUseCase addRestaurantUseCase)
        {
            var result = await addRestaurantUseCase.AddRestaurant(addRestaurantDto);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost("add-hotel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHotel (
            [FromBody] AddHotelDTO addHotelDTO,
            [FromServices] AddHotelUseCase addHotelUseCase)
        {
            var result = await addHotelUseCase.ExecuteAsync(addHotelDTO);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
    }
}
