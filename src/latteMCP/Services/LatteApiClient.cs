using System.Net.Http.Json;

namespace latteMCP.Services;

// Typed HttpClient to latteAPI (see ../../../docs/modules/latteMCP/architecture.md), shared by
// both plain surfaces (POST /login, GET /health) and every MCP tool. Returns raw
// HttpResponseMessages rather than deserialized bodies: callers need the exact status code to
// either pass it through as-is (MCP-REQ-004's /login wrapper) or translate it into an MCP tool
// error "with the same status/meaning" (see interfaces/mcp-tool-*.md).
public class LatteApiClient(HttpClient httpClient)
{
    public Task<HttpResponseMessage> GetHealthAsync(CancellationToken cancellationToken) =>
        httpClient.GetAsync("/health", cancellationToken);

    public Task<HttpResponseMessage> LoginAsync(Models.LoginRequest request, CancellationToken cancellationToken) =>
        httpClient.PostAsJsonAsync("/auth/login", request, LatteApiJsonOptions.Default, cancellationToken);

    public Task<HttpResponseMessage> GetMenuAsync(CancellationToken cancellationToken) =>
        httpClient.GetAsync("/menu", cancellationToken);

    // MCP-REQ-002 / ADR-0003: the caller's Authorization header is forwarded to latteAPI
    // unchanged; latteMCP never inspects or stores it.
    public Task<HttpResponseMessage> PlaceOrderAsync(
        string authorizationHeader, Models.CreateOrderRequest request, CancellationToken cancellationToken) =>
        SendAsync(HttpMethod.Post, "/orders", authorizationHeader, request, cancellationToken);

    public Task<HttpResponseMessage> GetOrderAsync(
        string authorizationHeader, Guid id, CancellationToken cancellationToken) =>
        SendAsync(HttpMethod.Get, $"/orders/{id}", authorizationHeader, body: null, cancellationToken);

    public Task<HttpResponseMessage> ListOrdersAsync(string authorizationHeader, CancellationToken cancellationToken) =>
        SendAsync(HttpMethod.Get, "/orders", authorizationHeader, body: null, cancellationToken);

    private async Task<HttpResponseMessage> SendAsync(
        HttpMethod method, string requestUri, string authorizationHeader, object? body, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, requestUri);
        request.Headers.TryAddWithoutValidation("Authorization", authorizationHeader);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: LatteApiJsonOptions.Default);
        }

        return await httpClient.SendAsync(request, cancellationToken);
    }
}
