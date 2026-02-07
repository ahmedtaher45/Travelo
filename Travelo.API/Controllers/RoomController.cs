using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> addRoom ([FromBody] RoomReqDTO roomReq)
        {
            var data = await roomService.CreateRoom(roomReq);
            return data.Success ? Ok(data) : BadRequest(data);
        }
        [HttpDelete("{roomId}")]
        public async Task<IActionResult> Delete (int roomId)
        {
            var data = await roomService.RemoveRoom(roomId);
            return data.Success ? Ok(data) : NotFound();
        }
        [HttpPut("{roomId}")]
        public async Task<IActionResult> Update (int roomId, [FromBody] RoomReqDTO roomReq)
        {
            var data = await roomService.UpdateRoom(roomId, roomReq);
            return data.Success ? Ok(data) : BadRequest(data);
        }

    }
}
