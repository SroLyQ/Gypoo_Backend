using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;

namespace GypooWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : Controller
    {
        private readonly HistoryService _historyService;
        public HistoryController(HistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public async Task<List<History>> Get()
        {
            return await _historyService.GetHistorysAsync();
        }

        [HttpGet("gethistory/{id}")]
        public async Task<History> GetRoom(string id)
        {
            return await _historyService.GetHistoryByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] History history)
        {
            await _historyService.CreateHistoryAsync(history);

            return CreatedAtAction(nameof(Get), new { id = history._id }, history);
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