// In Controllers/SecureCheckoutController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/secure/orders")]
[Authorize] // FIX: API2 - All endpoints now require authentication.
public class SecureCheckoutController : ControllerBase
{
    // A fake database for demonstration
    private static readonly List<Order> _orders = new List<Order>();
    private static readonly List<Product> _products = new List<Product> { new Product { Id = 1, Name = "Laptop", Price = 1200.00m } };
    private static int _nextOrderId = 1;

    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderRequest orderRequest)
    {
        // FIX: API3 (BOPLA / Mass Assignment) - Using a dedicated DTO.
        // The user can no longer set the price, status, or userId.

        // FIX: BOLA - Get the user's ID from their authenticated token (claim), not from the request body.
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"Authenticated user ID: {userIdString}");
        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Invalid user identifier in token.");
        }

        var newOrder = new Order
        {
            Id = _nextOrderId++,
            UserId = userId,
            OrderItems = new List<OrderItem>(),
            Status = "Pending", // Status is set by the server.
            CreatedAt = DateTime.UtcNow // Timestamp is set by the server.
        };

        decimal calculatedTotal = 0;
        foreach (var item in orderRequest.Items)
        {
            // Fetch product from the database to get the real price.
            var product = _products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null || item.Quantity <= 0)
            {
                // Invalid input, reject the request.
                return BadRequest("Invalid product or quantity.");
            }
            newOrder.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PricePerItem = product.Price // Price comes from the server, not the client.
            });
            calculatedTotal += product.Price * item.Quantity;
        }

        // The total is calculated securely on the server.
        newOrder.TotalAmount = calculatedTotal;
        _orders.Add(newOrder);

        // Return a safe DTO, not the internal database object.
        return Ok(new { OrderId = newOrder.Id, Total = newOrder.TotalAmount, Status = newOrder.Status });
    }

    [HttpGet("{id}")]
    public IActionResult GetOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        // FIX: API1 (BOLA) - Check if the authenticated user owns this order.
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (!int.TryParse(userIdString, out var userId) || (order.UserId != userId && !isAdmin))
        {
            // Either the user is not the owner or is not an admin.
            // Return a generic "NotFound" to avoid revealing the order's existence.
            return NotFound();
        }

        return Ok(order);
    }

    // Getting all orders is now restricted to admin users only.
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllOrders()
    {
        return Ok(_orders);
    }

    // Get jwt token for testing purpose
    [HttpGet("/api/token")]
    [AllowAnonymous]
    public IActionResult GetToken()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"), // Simulate user ID 1
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "User") // Change to "Admin" to test admin access
        };
        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: Constants.Jwt.Issuer,
            audience: Constants.Jwt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Constants.Jwt.Key)),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
        );
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(jwt);
        return Ok(new { token });
    }
        
}