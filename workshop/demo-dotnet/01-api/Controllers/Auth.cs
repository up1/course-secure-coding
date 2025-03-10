using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<AuthController> _logger;
    public AuthController(ILogger<AuthController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] UserLogin login)
    {
        if (login.Username == "user01" && login.Password == "password")
        {
            var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, "user-123"),
            new Claim(JwtRegisteredClaimNames.UniqueName, login.Username),
            new Claim("role", "user")
        };

            var token = new JwtSecurityToken(
                issuer: Constants.Jwt.Issuer,
                audience: Constants.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Jwt.Key)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }

        return await Task.FromResult(Unauthorized());
    }
}

public record UserLogin(string Username, string Password);
