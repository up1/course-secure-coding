# Demo of Broken Object Level Authorization (BOLA)

## Libraries
```
$dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
$dotnet add package Microsoft.EntityFrameworkCore.InMemory

$dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

$dotnet add package Microsoft.Extensions.Logging.Console
```

## Problem: Broken Object Level Authorization
* Insecure Example

```
// GET /orders/{orderId}
[HttpGet("{orderId}")]
public async Task<IActionResult> GetOrder(int orderId)
{
    var order = await _dbContext.Orders.FindAsync(orderId);

    if (order == null)
        return NotFound();

    return Ok(order); // ⚠️ Returns the order without checking ownership!
}
```

## Solution: Fix: Check if the logged-in user owns the resource
* Use policy-based authorization
```
[Authorize]
[HttpGet("{orderId}")]
public async Task<IActionResult> GetOrder(int orderId)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var order = await _dbContext.Orders
        .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

    if (order == null)
        return NotFound(); // Either not found or not authorized

    return Ok(order);
}

```

## Testing

Step 1 :: Create token
```
POST /auth
```
Step 2 :: Get order detail by id
* Authorization: Bearer <token>
```
GET /orders/1
GET /orders/2
GET /orders/3
```