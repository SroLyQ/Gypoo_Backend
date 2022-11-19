using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;
namespace GypooWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getComment(string id)
        {
            Comment comment = await _commentService.GetCommentAsync(id);
            return Ok(new { message = "Comment Found!" });
        }

        [HttpPost]
        public async Task<IActionResult> createComment([FromBody] Comment comment)
        {
            Comment _comment = await _commentService.CreateCommentAsync(comment);
            return Ok(new { message = "Commented!", comment = comment });
        }

        [HttpGet("hotel/{id}")]
        public async Task<IActionResult> getCommentByHotel(string id)
        {
            List<Comment> comments = await _commentService.GetCommentsByHotelId(id);
            return Ok(new { message = "Comments Found", comments = comments });
        }
    }
}