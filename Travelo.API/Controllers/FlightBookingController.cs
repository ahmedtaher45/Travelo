using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Payment;
using Travelo.Application.Services.Payment;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightBookingController : ControllerBase
    {
        private readonly IPaymentServices _payment;
        public FlightBookingController (IPaymentServices payment)
        {
            _payment=payment;
        }

        [HttpPost("BookFlight")]
        [Authorize]
        public async Task<IActionResult> BookFlight ([FromBody] FlightPaymentRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var result = await _payment.FlightBookingPayment(req, userId, baseUrl);
            return Ok(result);
        }

        [HttpGet("FlightSuccess/{paymentId}")]
        public async Task<IActionResult> FlightSuccess (int paymentId)
        {
            var result = await _payment.HandleSuccessAsync(paymentId);
            return !result!.Success ? BadRequest(result) : Ok(result);
        }

        [HttpGet("Cancel")]
        public IActionResult Cancel ()
        {
            return Ok("Payment Cancelled");
        }

    }
}
