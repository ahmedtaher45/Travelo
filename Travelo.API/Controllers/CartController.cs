using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Cart;

using Travelo.Application.UseCases.Carts;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AddToCartUseCase _addToCartUseCase;
        private readonly GetCartUseCase _getCartUseCase;
        private readonly RemoveCartItemUseCase _removeCartItemUseCase;
        private readonly RemoveFromCartUseCase _removeFromCartUseCase;

        public CartController(
            AddToCartUseCase addToCartUseCase,
            GetCartUseCase getCartUseCase,
            RemoveCartItemUseCase removeCartItemUseCase,
            RemoveFromCartUseCase removeFromCartUseCase)
        {
            _addToCartUseCase = addToCartUseCase;
            _getCartUseCase = getCartUseCase;
            _removeCartItemUseCase = removeCartItemUseCase;
            _removeFromCartUseCase = removeFromCartUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _getCartUseCase.ExecuteAsync(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddItem(AddToCartDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _addToCartUseCase.ExecuteAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("decrease-item")]
        public async Task<IActionResult> Remove(RemoveFromCartDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _removeFromCartUseCase.ExecuteAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var result = await _removeCartItemUseCase.ExecuteAsync(cartItemId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

}
