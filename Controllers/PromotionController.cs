using GypooWebAPI.Models;
using GypooWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GypooWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : Controller
    {
        private readonly PromotionService _promotionService;
        public PromotionController(PromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpPost]
        public async Task<IActionResult> createPromotion([FromBody] Promotion promotion)
        {
            Promotion _promotion = await _promotionService.createPromotion(promotion);
            if (_promotion == null)
            {
                return BadRequest(new { message = "Something went wrong!" });
            }
            return Ok(new { message = "Promotion Created!", promotino = _promotion });
        }

        [HttpGet("my_promotion/{id}")]
        public async Task<IActionResult> getMyPromotion(string id)
        {
            List<Promotion> _promotions = await _promotionService.getMyPromotions(id);
            if (_promotions.Count == 0)
            {
                return NotFound(new { message = "Promotion Not Found!", promotions = _promotions });
            }
            return Ok(new { message = "Promotions Found!", promotions = _promotions });
        }
    }

}