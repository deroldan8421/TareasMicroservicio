using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TareasMicroservicio.Models;

namespace TareasMicroservicio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Usuario fijo para la prueba técnica
            if (request.Usuario == "admin" && request.Clave == "admin123")
            {
                var jwtSettings = _config.GetSection("JwtSettings");
                var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("Falta JwtSettings:Key en appsettings.json");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Usuario)
                };

                var expiresInString = jwtSettings["ExpiresInMinutes"]
                    ?? throw new InvalidOperationException("Falta JwtSettings:ExpiresInMinutes en appsettings.json");

                var expiresInMinutes = double.Parse(expiresInString);

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenString });
            }

            return Unauthorized(new { error = "Credenciales incorrectas" });
        }
    }
}
