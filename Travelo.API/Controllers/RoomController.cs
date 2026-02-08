using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Room;
using Travelo.Application.Services.Room;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;

        public RoomController (IRoomService roomService)
        {
            this.roomService=roomService;
        }
        [HttpGet("{roomId}")]
        public async Task<IActionResult> Get (int roomId)
        {
            var res = await roomService.GetRoom(roomId);
            return res.Success ? Ok(res) : BadRequest(res);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Hotel")]
        public async Task<IActionResult> addRoom ([FromBody] RoomReqDTO roomReq)
        {            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var data = await roomService.CreateRoom(roomReq, userId, role);           
            return data.Success ? Ok(data) : BadRequest(data);
        }
        [HttpDelete("{roomId}")]
        [Authorize(Roles = "Admin, Hotel")]
        public async Task<IActionResult> Delete (int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var data = await roomService.RemoveRoom(roomId, userId, role);
            if (data.Success) return Ok(data);

            if (data.Message.Contains("Unauthorized")) return StatusCode(403, data);
            if (data.Message.Contains("not found")) return NotFound(data);
            return BadRequest(data);

        }
        [HttpPut("{roomId}")]
        [Authorize(Roles = "Admin, Hotel")]
        public async Task<IActionResult> Update (int roomId, [FromBody] RoomReqDTO roomReq)
        {                  
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var data = await roomService.UpdateRoom(roomId, roomReq, userId, role);
            return data.Success ? Ok(data) : BadRequest(data);

        }

    }
}
