using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.Services.Payment;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCheckoutController : ControllerBase
    {
        private readonly IPaymentServices _payment;

        public OrderCheckoutController (IPaymentServices payment)
        {
            _payment=payment;
        }
        [HttpPost("CartCheckout")]
        [Authorize]
        public async Task<IActionResult> CartCheckout ([FromBody] Travelo.Application.DTOs.Payment.CheckoutReq req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized();
            }
            var httpReq = $"{Request.Scheme}://{Request.Host.Value}";
            var result = await _payment.CartCheckout(req, userId, httpReq);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("CartSuccess/{orderId}")]
        public async Task<IActionResult> HandelCartSuccessAsync (int orderId)
        {
            var result = await _payment.HandelCartSuccessAsync(orderId);
            return !result!.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Cancel")]
        public IActionResult Cancel ()
        {
            return Ok("Payment Cancelled");
        }
    }
}
