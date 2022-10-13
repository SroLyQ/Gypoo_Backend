using System;
using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;

namespace GypooWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly HotelService _hotelService;

        public HotelController(HotelService hotelServices)
        {
            _hotelService = hotelServices;
        }

        [HttpGet]
        public async Task<List<Hotel>> Get()
        {
            return await _hotelService.GetHotelsAsync();
        }

        [HttpGet("gethotel/{id}")]
        public async Task<Hotel> GetHotel(string id)
        {
            return await _hotelService.GetHotelByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hotel hotel)
        {
            await _hotelService.CreateAsync(hotel);
            return CreatedAtAction(nameof(Get), new { id = hotel.Id }, hotel);
        }

        [HttpPut("id")]
        public async Task<IActionResult> AddRoomToHotel(string id, [FromBody] string roomId)
        {
            await _hotelService.AddRoomToHotelAsync(id, roomId);
            return NoContent();
        }
    }
}