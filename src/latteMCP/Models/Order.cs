namespace latteMCP.Models;

// Mirrors latteAPI's Models/Order.cs exactly (MCP-REQ-001/002) — this module doesn't redefine
// latteAPI's contract, only carries it through the MCP tool-call/tool-result envelope.

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
    public Guid Id { get; init; }
    public required List<OrderLine> Items { get; init; }
    public OrderStatus Status { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public decimal Total { get; init; }
    public required string CreatedBy { get; init; }
}
