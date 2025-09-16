public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; } // e.g., "Customer" or "Admin"
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key to User
    public List<OrderItem> OrderItems { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } // e.g., "Pending", "Shipped"
    public DateTime CreatedAt { get; set; }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
}