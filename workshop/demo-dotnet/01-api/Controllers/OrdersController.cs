using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IAuthorizationService _auth;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(AppDbContext db, ILogger<OrdersController> logger, IAuthorizationService auth)
    {
        _db = db;
        _auth = auth;
        _logger = logger;
    }

    [HttpGet("{orderId}")]
    [Authorize]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        _logger.LogInformation($"Order ID: {orderId}");
        var result = await _auth.AuthorizeAsync(User, orderId, "OrderOwnerPolicy");

        if (!result.Succeeded)
            return Forbid();

        var order = await _db.Orders.FindAsync(orderId);
        _logger.LogInformation($"Order Details: {order}");

        return Ok(order);
    }
}
