using Microsoft.AspNetCore.Mvc;
using GypooWebAPI.Services;
using GypooWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace GypooWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public async Task<UserNoPW> GetUser(string id)
        {
            return await _userService.GetUserByIdAsync(id);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDTO request)
        {
            var _user = await _userService.registerAsync(request);
            return CreatedAtAction("register", new { user = _user });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDTO req)
        {
            var token = await _userService.loginAsync(req);
            if (token == "false")
            {
                return BadRequest(new { message = "Wrong Password!" });
            }
            else if (token == "UsernameFalse")
            {
                return BadRequest(new { message = "Wrong Username!" });
            }

            return Ok(new { token = token });
        }

        [HttpGet("token")]
        public async Task<ActionResult<string>> validateToken()
        {
            string token = Request.Headers["x-access-token"];
            Console.WriteLine(token);
            if (token == "" || token == null)
            {
                return Unauthorized(new { message = "Header Token missing!" });
            }
            else
            {
                bool isValid = _userService.validateToken(token);
                if (isValid)
                {
                    return Ok(new { token = token });
                }
                else
                {
                    return Unauthorized(new { message = "Token invalid" });
                }
            }

        }
    }
}