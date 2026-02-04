using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.Services.Booking;

namespace Travelo.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController (IBookingService bookingService)
        {
            _bookingService=bookingService;
        }

        [Authorize]
        [HttpGet("my-trips")]
        public async Task<IActionResult> GetMyTrips ()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingService.GetMyTripsAsync(userId);

            return Ok(result);
        }

        [HttpGet("trips/{id}")]
        public async Task<IActionResult> GetTripDetails (int id)
        {
            var result = await _bookingService.GetDetailsAsync(id);

            return result==null ? NotFound() : Ok(result);
        }

        //we dont need it Booking Create  after Payment for Flight in sucsses
        //[HttpPost("flight")]
        //public async Task<IActionResult> CreateFlightBooking ([FromBody] CreateBookingDto dto)
        //{
        //    var result = await _bookingService.CreateFlightBookingAsync(dto);
        //    return Ok(result);
        //}


        [HttpPost("{bookingId}/confirm")]
        public async Task<IActionResult> ConfirmBooking (int bookingId)
        {
            var result = await _bookingService.ConfirmBookingAsync(bookingId);
            return Ok(result);
        }

        [HttpPost("{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking (int bookingId)
        {
            await _bookingService.CancelBookingAsync(bookingId);
            return NoContent();
        }
        [HttpGet("GetAllRoomBooking")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActionResultAsync ()
        {
            var res = await _bookingService.GetAllRoomBookingAsync();
            return res.Success ? Ok(res) : BadRequest(res);
        }
        [HttpGet("GetAllFlightBooking")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFlightBooking ()
        {
            var res = await _bookingService.GetAllFlightBookingAsync();
            return res.Success ? Ok(res) : BadRequest(res);
        }
        [HttpGet("GetHotelBookingRooms{hotelId}")]
        [Authorize(Roles = "Hotel")]
        public async Task<IActionResult> GetAllHotelBookingRooms (int hotelId)
        {
            var res = await _bookingService.GetHotelRoomBookingAsync(hotelId);
            return res.Success ? Ok(res) : NotFound(res);
        }
    }
}
