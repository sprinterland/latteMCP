namespace latteAPI.Models;

public enum DrinkSize
{
    Small,
    Medium,
    Large
}

public enum OrderStatus
{
    Received,
    Preparing,
    Ready,
    Completed
}

public record OrderLine(int MenuItemId, DrinkSize Size, int Quantity);

public record CreateOrderRequest(List<OrderLine> Items);

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required List<OrderLine> Items { get; init; }
    public OrderStatus Status { get; set; } = OrderStatus.Received;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public decimal Total { get; set; }
    public required string CreatedBy { get; init; }
}
