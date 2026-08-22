using latteAPI.Models;

namespace latteAPI.Data;

// Kept in sync with the "Seed / Example Data" section of ../../docs/modules/latteAPI/domain-model.md.
public static class MenuCatalog
{
    public static readonly IReadOnlyList<MenuItem> Items =
    [
        new(1, "Latte", "Espresso with steamed milk", 4.25m),
        new(2, "Cappuccino", "Espresso with steamed milk foam", 4.25m),
        new(3, "Americano", "Espresso with hot water", 3.25m),
        new(4, "Mocha", "Espresso with chocolate and steamed milk", 4.75m),
        new(5, "Espresso", "A double shot, no milk", 2.75m)
    ];

    public static readonly IReadOnlyDictionary<DrinkSize, decimal> SizeSurcharge = new Dictionary<DrinkSize, decimal>
    {
        [DrinkSize.Small] = 0m,
        [DrinkSize.Medium] = 0.60m,
        [DrinkSize.Large] = 1.20m
    };
}
