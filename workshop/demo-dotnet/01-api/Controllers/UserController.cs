using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    // ❌ Insecure: Updates all fields from client input
    [HttpPut("update-vulnerable/{id}")]
    public async Task<IActionResult> UpdateUserVulnerable(int id, [FromBody] User updatedUser)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        // ⚠️ Entire entity is replaced — even sensitive fields!
        _db.Entry(user).CurrentValues.SetValues(updatedUser);
        await _db.SaveChangesAsync();

        return Ok("Updated (insecure)");
    }

    // ✅ Secure: Update only allowed fields
    [HttpPut("update-secure/{id}")]
    public async Task<IActionResult> UpdateUserSecure(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        // ✅ Only update whitelisted fields
        user.Email = dto.Email;

        await _db.SaveChangesAsync();
        return Ok("Updated (secure)");
    }
}

public class UpdateUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}
