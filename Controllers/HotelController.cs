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
        [HttpGet("type")]
        public async Task<IActionResult> GetType([FromQuery] string type)
        {
            List<Hotel> hotels = await _hotelService.GetHotelByTypeAsync(type);
            return Ok(new
            {
                hotels = hotels
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(string id)
        {
            Console.WriteLine(id);
            Hotel hotel = await _hotelService.GetHotelByIdAsync(id);
            return Ok(new { hotel = hotel });

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hotel hotel)
        {
            await _hotelService.CreateAsync(hotel);
            return CreatedAtAction(nameof(Post), new { id = hotel.Id }, hotel);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Hotel hotel)
        {
            Hotel updatedHotel = await _hotelService.updateHotelByIdAsync(id, hotel);
            if (updatedHotel == null)
            {
                return BadRequest(new { message = "Wrong id or Body key(s) missing!" });
            }
            return Ok(new { message = "Updated", hotel = updatedHotel });
        }
        [HttpPut("addRoomTo/{id}")]
        public async Task<IActionResult> AddRoomToHotel(string id, [FromBody] string roomId)
        {
            await _hotelService.AddRoomToHotelAsync(id, roomId);
            return Ok(new { message = "Room Added" });
        }

        [HttpGet("myHotel/{id}")]
        public async Task<IActionResult> GetMyHotel(string id)
        {
            List<Hotel> hotels = await _hotelService.getHotelByOwnerId(id);
            return Ok(new { message = "My Hotels Found!", hotels = hotels });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchHotel([FromQuery(Name = "checkIn")] string checkIn, [FromQuery(Name = "checkOut")] string checkOut)
        {
            List<Hotel> hotels = await _hotelService.getAvailableHotel(checkIn, checkOut);
            return Ok(new { message = "Found Hotels", hotels = hotels });
        }
    }
}