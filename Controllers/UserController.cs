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
        public async Task<ActionResult<string>> Register([FromBody] UserDTO request)
        {

            if (request.password != request.confirmPassword)
            {
                return BadRequest(new { message = "Password must match!" });
            }
            var _user = await _userService.registerAsync(request);
            if (_user == null)
            {
                return BadRequest(new { message = "User Already Exist!" });
            }
            return CreatedAtAction("register", new { user = _user, token = _user.token });
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
        [HttpGet("username/{id}")]
        public async Task<ActionResult<string>> getUsername(string id)
        {
            string username = await _userService.getUsername(id);
            if (username == "Not Found")
            {
                return BadRequest(new { message = "User Not Found!", username = "" });
            }
            return Ok(new { message = "Ok User Found!", username = username });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> updateProfile(string id, [FromBody] UserUpdate userData)
        {
            UserNoPW user = await _userService.updateUser(id, userData);

            return Ok(new { message = "User updated!", user = user });
        }
    }
}