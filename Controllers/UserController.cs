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
        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await _userService.GetUsersAsync();
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDTO request)
        {
            var _user = await _userService.registerAsync(request);
            return CreatedAtAction("register", _user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDTO req)
        {
            var token = await _userService.loginAsync(req);
            if (token == "false")
            {
                return BadRequest("Wrong Password!");
            }
            else if (token == "UsernameFalse")
            {
                return BadRequest("Wrong Username!");
            }
            return token;
        }

        [HttpGet("token")]
        public async Task<ActionResult<string>> validateToken()
        {
            string token = Request.Headers["x-access-token"];
            Console.WriteLine(token);
            if (token == "" || token == null)
            {
                return Unauthorized("Header Token missing!");
            }
            else
            {
                bool isValid = _userService.validateToken(token);
                if (isValid)
                {
                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Token invalid");
                }
            }

        }
    }
}