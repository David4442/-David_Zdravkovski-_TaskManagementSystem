using Exam.Data;
using Exam.Models;
using Exam.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public UserController(IUserService userService,JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            await _userService.RegisterUserAsync(userDto);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userService.AuthenticateAsync(userDto.Email, userDto.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
