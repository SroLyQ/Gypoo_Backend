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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Room room)
        {
            Room updateRoom = await _roomService.updateRoomByIdAsync(id, room);
            if (updateRoom == null)
            {
                return BadRequest(new { message = "Room : Wrong id or Body key(s) missing!" });
            }
            return Ok(new { message = "Updated", room = updateRoom });
            // await _roomService.AddDetailToRoomAsync(id, room);

        }

        [HttpPut("booking/{id}")]
        public async Task<IActionResult> PutBooking(string id, List<string> dateBooking, int numBooking)
        {
            await _roomService.BookingHotel(id, dateBooking, numBooking);
            // Room updateRoom = await _roomService.updateRoomByIdAsync(id, room);

            // if (updateRoom == null)
            // {
            //     return BadRequest(new { message = "Room : Wrong id or Body key(s) missing!" });
            // }
            return Ok(new { message = "Updated" });


        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteToRoom(string id)
        // {
        //     var RoomDelete = await _roomService.GetRoomByIdAsync(id);
        //     if (RoomDelete == null)
        //     {
        //         return NotFound($"Room with ID = {id} not found");
        //     }
        //     return await _roomService.DeleteRoomAsync(id);     

        // }

    }

}