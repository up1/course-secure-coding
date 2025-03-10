using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TestDb"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderOwnerPolicy", policy =>
        policy.Requirements.Add(new OrderOwnerRequirement()));
});

builder.Services.AddScoped<IAuthorizationHandler, OrderOwnerHandler>();

// JWT Settings

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Jwt.Key));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Constants.Jwt.Issuer,
        ValidAudience = Constants.Jwt.Audience,
        IssuerSigningKey = key
    };
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🔁 Seed Initial Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Orders.AddRange(
        new Order { Id = 1, UserId = "user-123", ProductName = "Laptop", TotalAmount = 1500 },
        new Order { Id = 2, UserId = "user-456", ProductName = "Phone", TotalAmount = 800 },
        new Order { Id = 3, UserId = "user-123", ProductName = "Monitor", TotalAmount = 300 }
    );

    db.SaveChanges();
}


app.Run();
