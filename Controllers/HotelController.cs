using System;
using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;

namespace GypooWebAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly HotelService _mongoDBService;

        public HotelController(HotelService hotelServices)
        {
            _mongoDBService = hotelServices;
        }

        [HttpGet]
        public async Task<List<Hotel>> Get()
        {
            return await _mongoDBService.GetHotelsAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hotel hotel)
        {
            await _mongoDBService.CreateAsync(hotel);
            return CreatedAtAction(nameof(Get), new { id = hotel.Id }, hotel);
        }

        [HttpPut("id")]
        public async Task<IActionResult> AddRoomToHotel(string id, [FromBody] string roomId)
        {
            await _mongoDBService.AddRoomToHotelAsync(id, roomId);
            return NoContent();
        }
    }
}