using AuthService.Application.DTOs;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RegisterService _registerService;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, RegisterService registerService, IJwtService jwtService)
        {
            _context = context;
            _registerService = registerService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            var result = await _registerService.RegisterAsync(model.Username, model.Email, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return Unauthorized("Invalid username or password");

            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }
}
