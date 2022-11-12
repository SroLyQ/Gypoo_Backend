using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;
namespace GypooWebAPI.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : Controller
    {
        private readonly UploadService _uploadService;
        public UploadController(UploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> uploadPicture(List<IFormFile> files)
        {
            var res = await _uploadService.uploadPictures(files);
            if (res[0] != "IO ERROR")
            {
                return Ok(new { message = "OK", imgPath = res });
            }
            else
            {
                return StatusCode(400, new { message = "Internal Error" });
            }
        }

    }
}