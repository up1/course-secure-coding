using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;

    public AuthController(ILogger<AuthController> logger, AppDbContext db, ITokenService tokenService)
    {
        _logger = logger;
        _db = db;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] UserLogin login)
    {
        if (login.Username == "user01" && login.Password == "password")
        {
            var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, "1"), // Hard code user id
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserLogin login)
    {
        if (await _db.Users.AnyAsync(u => u.Username == login.Username))
            return BadRequest("Username already exists");

        var user = new User
        {
            Username = login.Username,
            PasswordHash = HashPassword(login.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // return Ok("User registered");

        // Debug mode
        return Ok(new { user });
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }


    [HttpPost("login-secure")] // ✅ Secure Authentication
    public async Task<IActionResult> LoginSecure(UserLogin login)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

        if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
            return Unauthorized(new { error = "Invalid username or password" });

        var token = _tokenService.GenerateToken(user.Id.ToString(), user.Username);
        return Ok(new { token });
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

}

public record UserLogin(string Username, string Password);
