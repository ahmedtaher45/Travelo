using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.Services.Payment;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBokingController : ControllerBase
    {
        private readonly IPaymentServices payment;

        public RoomBokingController (IPaymentServices payment)
        {
            this.payment=payment;
        }
        [HttpPost("CheckOut")]
        [Authorize]
        public async Task<IActionResult> CreateRoomBookingPayment ([FromBody] Travelo.Application.DTOs.Payment.RoomBookingPaymentReq req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized();
            }
            var httpReq = $"{Request.Scheme}://{Request.Host.Value}";
            var result = await payment.CreateRoomBookingPayment(req, userId, httpReq);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Success/{paymentId}")]
        public async Task<IActionResult> HandelSuccessAsync (int paymentId)
        {
            var result = await payment.HandleSuccessAsync(paymentId);
            return !result!.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Cancel")]
        public IActionResult Cancel ()
        {
            return Ok("Payment Cancelled");
        }

    }
}
