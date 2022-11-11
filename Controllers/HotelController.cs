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
        public async Task<IActionResult> Get()
        {
            List<Hotel> hotels = await _hotelService.GetHotelsAsync();
            return Ok(new
            {
                hotels = hotels
            });
        }

        [HttpGet("gethotel/{id}")]
        public async Task<IActionResult> GetHotel(string id)
        {
            Hotel hotel = await _hotelService.GetHotelByIdAsync(id);
            return Ok(new { hotel = hotel });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hotel hotel)
        {
            await _hotelService.CreateAsync(hotel);
            return CreatedAtAction(nameof(Get), new { id = hotel.Id }, hotel);
        }

        [HttpPut("addRoom/{id}")]
        public async Task<IActionResult> AddRoomToHotel(string id, [FromBody] string roomId)
        {
            await _hotelService.AddRoomToHotelAsync(id, roomId);
            return NoContent();
        }
    }
}