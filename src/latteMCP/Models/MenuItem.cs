namespace latteMCP.Models;

// Mirrors latteAPI's GET /menu response shape exactly (MCP-REQ-001) — see
// ../../latteAPI/interfaces/get-menu.md.

public record MenuItem(int Id, string Name, string Description, decimal BasePrice);

public record MenuResponse(List<MenuItem> Items, Dictionary<DrinkSize, decimal> SizeSurcharge);
