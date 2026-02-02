using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Wishlist;
using Travelo.Application.Services.Wishlist;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService wishlistService;

        public WishlistController (IWishlistService wishlistService)
        {
            this.wishlistService=wishlistService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWishlist (
            [FromBody] WishlistRequestDTO dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await wishlistService
                .CreateWishlistAsync(userId, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList ()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await wishlistService.GetWishlistAsync(userId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetWishlistsWithDetails ()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await wishlistService
                .GetWishlistWithDetalsAsync(userId);

            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlist (
            int id,
            [FromBody] WishlistRequestDTO dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await wishlistService
                .UpdateWishlistAsync(userId, dto, id);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist (int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await wishlistService
                .DeleteWishlistAsync(userId, id);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishListDetals (int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value!;
            var result = await wishlistService.GetWishlistWithDetalsAsync(userId, id);
            return (result.Success ? Ok(result) : NotFound());
        }
    }
}
