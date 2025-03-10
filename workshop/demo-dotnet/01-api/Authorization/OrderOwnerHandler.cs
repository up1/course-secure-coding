using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class OrderOwnerHandler : AuthorizationHandler<OrderOwnerRequirement, int>
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderOwnerHandler> _logger;

    public OrderOwnerHandler(AppDbContext db, ILogger<OrderOwnerHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OrderOwnerRequirement requirement,
        int orderId)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"User ID: {userId}");


        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order != null)
        {
            context.Succeed(requirement);
            return;
        }
        context.Fail();
    }
}
