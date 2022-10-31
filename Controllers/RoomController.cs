using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;

namespace GypooWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;
        public RoomController(RoomService roomServices)
        {
            _roomService = roomServices;
        }

        [HttpGet]
        public async Task<List<Room>> Get()
        {
            return await _roomService.GetRoomsAsync();
        }

        [HttpGet("getroom/{id}")]
        public async Task<Room> GetRoom(string id)
        {
            return await _roomService.GetRoomByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Room room)
        {
            await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(Get), new { id = room.idRoom }, room);
        }

        [HttpPut("id")]
        public async Task<IActionResult> AddDetailToRoom(string id, [FromBody] string roomId)
        {
            await _roomService.AddDetailToRoomAsync(id, roomId);
            return NoContent();
        }
    }

}