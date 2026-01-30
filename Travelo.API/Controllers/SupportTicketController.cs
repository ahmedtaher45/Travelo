using Travelo.Application.DTOs.SupportTicket;
using Travelo.Application.UseCases.SupportTicket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Diagnosis.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
   
    public class SupportTicketController : ControllerBase
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateSupportTicketAsync([FromBody]SupportTicketDTO supportTicketDTO, [FromServices]AddSupportTicketUseCase supportTicketUseCase)
        {

            if (supportTicketDTO == null)
                return BadRequest("SupportTicketDTO cannot be null.");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
            await supportTicketUseCase.CreateSupportTicketAsync(userId, supportTicketDTO);

            return Ok(new { Message = "Support ticket created successfully." });

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<GetSupportTicketDTO>>> GetSupportTicketsAsync([FromServices]GetSupportTicketsUseCase getSupportTicketsUseCase)
        {
            
            var tickets = await getSupportTicketsUseCase.GetSupportTicketsAsync();
            return Ok(tickets);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("content/{ticketId}")]
        public async Task<ActionResult<List<GetSupportTicketDTO>>> GetSupportTicketContentAsync(
            [FromServices] GetSupportTicketContentUseCase getSupportTicketUseCase,
            [FromRoute] int ticketId)
        {
            var tickets = await getSupportTicketUseCase.ExecuteAsync(ticketId);
            return Ok(tickets);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("reply")]
        public async Task<IActionResult> AddSupportTicketReplyAsync([FromBody] AddSupportTicketReplyDTO supportTicketReplyDTO , [FromServices]AddSuportTicketReplyUseCase addSuportTicketReplyUseCase)
        {
            await addSuportTicketReplyUseCase.AddSupportTicketReplyAsync(supportTicketReplyDTO);
            return Ok("Reply added Successfully");
        }
        [Authorize]
        [HttpGet("reply")]
        public async Task<IActionResult> GetLatestReplyByUserAsync([FromServices]GetSuportTicketReplyUseCase getSuportTicketReplyUseCase)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
            var reply = await getSuportTicketReplyUseCase.GetLatestReplyByUserAsync(userId);

            if (reply == null)
                return NotFound("No reply found for this user.");

            return Ok(reply);
        }
        [Authorize]
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetSupportTicketAsync([FromServices] GetSupportTicketUseCase getSupportTicketUseCase, [FromRoute] int ticketId)
        {
            var ticket = await getSupportTicketUseCase.GetSupportTicketByIdAsync(ticketId);
            if (ticket == null) return NotFound("Support ticket not found.");

            return Ok(ticket);
        }
    }
}
