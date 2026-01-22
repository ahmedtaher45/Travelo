using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateRoomBookingPayment ([FromBody] Travelo.Application.DTOs.Payment.RoomBookingPaymentReq req)
        {
            var userId = HttpContext.User.FindFirst("uid")?.Value;
            var httpReq = $"{Request.Scheme}://{Request.Host.Value}";
            var result = await payment.CreateRoomBookingPayment(req, userId!, httpReq);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Success/{paymentId}")]
        public async Task<IActionResult> HandelSuccessAsync (int paymentId)
        {
            var result = await payment.HandelSuccessAsync(paymentId);
            return !result!.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("Cancel")]
        public IActionResult Cancel ()
        {
            return Ok("Payment Cancelled");
        }

    }
}
