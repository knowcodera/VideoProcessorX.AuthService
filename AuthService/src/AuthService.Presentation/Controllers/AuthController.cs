using AuthService.Application.DTOs;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoProcessorX.Domain.Entities;

namespace AuthService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly RegisterService _registerService;

        public AuthController(AppDbContext context, IConfiguration configuration, RegisterService registerService)
        {
            _context = context;
            _configuration = configuration;
            _registerService = registerService;
        }

        // Endpoint para registrar usuário (opcional para MVP, você pode inserir direto no BD se quiser)
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

        // Endpoint para Login (gera token JWT)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            // Verifica hash da senha
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return Unauthorized("Invalid username or password");

            // Gera token JWT
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Função auxiliar para gerar token
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Claims - aqui podemos adicionar mais (e.g. roles)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("username", user.Username)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
