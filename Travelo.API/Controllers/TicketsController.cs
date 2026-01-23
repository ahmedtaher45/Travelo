using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Ticket;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Ticket;

namespace Travelo.API.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var result = await _ticketService.CreateTicketAsync(dto);
            return Ok(result);
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketById(int ticketId)
        {
            var result = await _ticketService.GetTicketByIdAsync(ticketId);
            return Ok(result);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetTicketByBookingId(int bookingId)
        {
            var result = await _ticketService.GetTicketByBookingIdAsync(bookingId);
            return Ok(result);
        }

        [HttpPost("{ticketId}/confirm")]
        public async Task<IActionResult> ConfirmTicket(int ticketId)
        {
            var result = await _ticketService.ConfirmTicketAsync(ticketId);
            return Ok(result);
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            await _ticketService.DeleteTicketAsync(ticketId);
            return NoContent();
        }
    }
}
