using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("logs")]
public class LogsController : ControllerBase
{
    private readonly AppDbContext _db;

    public LogsController(AppDbContext db)
    {
        _db = db;
    }

    // ✅ Rate-limited endpoint
    [HttpGet("recent")]
    [EnableRateLimiting("fixed")] // Apply limiter
    public async Task<IActionResult> GetRecentLogs()
    {
        var recent = await _db.Logs
            .OrderByDescending(l => l.Timestamp)
            .Take(10)
            .ToListAsync();

        return Ok(recent); 
    }
}
