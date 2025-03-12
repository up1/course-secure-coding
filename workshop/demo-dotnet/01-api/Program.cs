using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TestDb"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderOwnerPolicy", policy =>
        policy.Requirements.Add(new OrderOwnerRequirement()));
});

builder.Services.AddScoped<IAuthorizationHandler, OrderOwnerHandler>();

builder.Services.AddScoped<ITokenService, TokenService>();

// JWT Settings
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Jwt.Key));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Constants.Jwt.Issuer,
            ValidAudience = Constants.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Constants.Jwt.Key))
        };
});

// Rate Limiting Configuration
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(10);      // 10-second window
        limiter.PermitLimit = 5;                         // Max 5 requests per window
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 0;                          // Allow 2 requests to queue
    });
    
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
    };
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

app.UseRateLimiter(); // 🔐 Enable rate limiting
app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Add Swagger UI


// 🔁 Seed Initial Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Add test data for Orders
    db.Orders.AddRange(
        new Order { Id = 1, UserId = "1", ProductName = "Laptop", TotalAmount = 1500 },
        new Order { Id = 2, UserId = "user-456", ProductName = "Phone", TotalAmount = 800 },
        new Order { Id = 3, UserId = "1", ProductName = "Monitor", TotalAmount = 300 }
    );
    db.SaveChanges();

    // Seed test data for Logs
    for (int i = 1; i <= 100; i++)
    {
        db.Logs.Add(new LogEntry
        {
            Message = $"Log #{i}",
            Timestamp = DateTime.UtcNow.AddMinutes(-i)
        });
    }
    Console.WriteLine("Seeding logs done.");

    db.SaveChanges();
}

app.Run();