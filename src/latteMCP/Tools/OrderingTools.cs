using System.ComponentModel;
using System.Net.Http.Json;
using latteMCP.Models;
using latteMCP.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace latteMCP.Tools;

// MCP-REQ-001: exposes latteAPI's ordering operations as MCP tools. `LatteApiClient` and
// `IHttpContextAccessor` are resolved from DI per request (see McpServerToolAttribute's binding
// rules) rather than supplied as tool arguments, so neither appears in the generated input
// schema.
[McpServerToolType]
public static class OrderingTools
{
    [McpServerTool(Name = "get_menu")]
    [Description("Lists the drinks on the menu with their base prices and the per-size surcharge. No login required.")]
    public static async Task<MenuResponse> GetMenu(LatteApiClient latteApi, CancellationToken cancellationToken)
    {
        var response = await latteApi.GetMenuAsync(cancellationToken);
        return await ReadResultAsync<MenuResponse>(response, cancellationToken);
    }

    [McpServerTool(Name = "place_order")]
    [Description("Places a new order for one or more drinks. Requires a logged-in waitress: attach the token from POST /login as this MCP request's 'Authorization: Bearer <token>' header.")]
    public static async Task<Order> PlaceOrder(
        LatteApiClient latteApi,
        IHttpContextAccessor httpContextAccessor,
        [Description("The drinks to order.")] List<OrderLine> items,
        CancellationToken cancellationToken)
    {
        var authorization = GetRequiredAuthorizationHeader(httpContextAccessor);
        var response = await latteApi.PlaceOrderAsync(authorization, new CreateOrderRequest(items), cancellationToken);
        return await ReadResultAsync<Order>(response, cancellationToken);
    }

    [McpServerTool(Name = "get_order")]
    [Description("Looks up a single order by id. Requires a logged-in waitress: attach the token from POST /login as this MCP request's 'Authorization: Bearer <token>' header.")]
    public static async Task<Order> GetOrder(
        LatteApiClient latteApi,
        IHttpContextAccessor httpContextAccessor,
        [Description("The id of the order to look up.")] Guid id,
        CancellationToken cancellationToken)
    {
        var authorization = GetRequiredAuthorizationHeader(httpContextAccessor);
        var response = await latteApi.GetOrderAsync(authorization, id, cancellationToken);
        return await ReadResultAsync<Order>(response, cancellationToken);
    }

    [McpServerTool(Name = "list_orders")]
    [Description("Lists all orders, most recent first. Requires a logged-in waitress: attach the token from POST /login as this MCP request's 'Authorization: Bearer <token>' header.")]
    public static async Task<List<Order>> ListOrders(
        LatteApiClient latteApi,
        IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var authorization = GetRequiredAuthorizationHeader(httpContextAccessor);
        var response = await latteApi.ListOrdersAsync(authorization, cancellationToken);
        return await ReadResultAsync<List<Order>>(response, cancellationToken);
    }

    // MCP-REQ-003 / MCP-RULE-001: read fresh off the current HTTP request every call — nothing
    // is cached — and fail before latteAPI is ever reached if it's missing.
    private static string GetRequiredAuthorizationHeader(IHttpContextAccessor httpContextAccessor)
    {
        var header = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(header))
        {
            throw new McpException(
                "Not logged in: this request has no Authorization header. Call POST /login first and " +
                "attach the returned token as this MCP request's 'Authorization: Bearer <token>' header.");
        }

        return header;
    }

    // Translates a latteAPI error response into an MCP tool error "with the same status/meaning"
    // (see interfaces/mcp-tool-*.md) instead of a generic failure.
    private static async Task<T> ReadResultAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var detail = string.IsNullOrWhiteSpace(errorBody) ? string.Empty : $": {errorBody}";
            throw new McpException($"latteAPI returned {(int)response.StatusCode} {response.ReasonPhrase}{detail}");
        }

        return await response.Content.ReadFromJsonAsync<T>(LatteApiJsonOptions.Default, cancellationToken)
            ?? throw new McpException("latteAPI returned an empty response body.");
    }
}
