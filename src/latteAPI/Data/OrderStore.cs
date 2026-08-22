using System.Collections.Concurrent;
using latteAPI.Models;

namespace latteAPI.Data;

public class OrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Order Add(Order order)
    {
        _orders[order.Id] = order;
        return order;
    }

    public Order? Get(Guid id) => _orders.GetValueOrDefault(id);

    public IEnumerable<Order> GetAll() => _orders.Values.OrderByDescending(o => o.CreatedAt);
}
