public class Order
{
    public int Id { get; set; }
    public required string UserId { get; set; } // Owner's ID
    public required string ProductName { get; set; }
    public decimal TotalAmount { get; set; }

    public string GetOrderSummary()
    {
        return $"Order ID: {Id}, User: {UserId}, Product: {ProductName}, Total Amount: {TotalAmount:C}";
    }
}
