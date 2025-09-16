// InControllers/InsecureCheckoutController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/insecure/orders")]
public class InsecureCheckoutController : ControllerBase
{
    // A fake database for demonstration
    private static readonly List<Order> _orders = new List<Order>();
    private static int _nextOrderId = 1;

    // FLAW 1: Taking a complex object directly and trusting its properties.
    // FLAW 2: The user can specify their own UserId.
    // FLAW 3: No authentication or authorization is required to create an order.
    [HttpPost]
    public IActionResult CreateOrder([FromBody] Order order)
    {
        try
        {
            // The user can set any TotalAmount, Status, or even the CreatedAt timestamp.
            // There's no server-side validation of the order total.
            order.Id = _nextOrderId++;
            _orders.Add(order);

            // Returns the entire sensitive order object.
            return Ok(order);
        }
        catch (Exception ex)
        {
            // FLAW 4: Leaking internal server information in error messages.
            return StatusCode(500, $"An internal error occurred: {ex.Message}");
        }
    }

    // FLAW 5: Anyone can retrieve any order by guessing the ID.
    // FLAW 6: No authentication check.
    [HttpGet("{id}")]
    public IActionResult GetOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order); // Returns the order, even if it belongs to another user.
    }
}