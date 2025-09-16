// InControllers/InsecureCheckoutController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/insecure/orders")]
public class InsecureCheckoutController : ControllerBase
{
    // A fake database for demonstration
    private static readonly List<Order> _orders = new List<Order>();
    private static int _nextOrderId = 1;

    [HttpPost]
    public IActionResult CreateOrder([FromBody] Order order)
    {
        try
        {
            order.Id = _nextOrderId++;
            _orders.Add(order);

            // Returns the entire sensitive order object.
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An internal error occurred: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    // Get all orders (insecure, exposes all data)
    [HttpGet]
    public IActionResult GetAllOrders()
    {
        return Ok(_orders);
    }
}