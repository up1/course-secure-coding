public class CreateOrderRequest
{
    // User only needs to send a list of products and quantities.
    public List<CreateOrderItemRequest> Items { get; set; }
}

public class CreateOrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}