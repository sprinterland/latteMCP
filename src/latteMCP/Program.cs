using latteMCP;
using latteMCP.Models;
using latteMCP.Services;

var builder = WebApplication.CreateBuilder(args);

// Generated OpenAPI document for this module's two plain REST endpoints (ADR-0005). The MCP
// tools on /mcp are covered by the MCP protocol's own tool-discovery instead, not OpenAPI.
builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    foreach (var converter in LatteApiJsonOptions.Default.Converters)
    {
        options.SerializerOptions.Converters.Add(converter);
    }
});

// MCP-REQ-002/003: tools read the caller's Authorization header straight off the current HTTP
// request (see Tools/OrderingTools.cs) — nothing is cached server-side (MCP-RULE-001).
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<LatteApiClient>(client =>
{
    var baseUrl = builder.Configuration["LatteApi:BaseUrl"]
        ?? throw new InvalidOperationException("Configuration key 'LatteApi:BaseUrl' is missing.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(serializerOptions: LatteApiJsonOptions.Default);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// MCP-REQ-005 / ADR-0004: healthy only if latteAPI is also reachable — a dependency check, not a
// plain liveness check.
app.MapGet("/health", async (LatteApiClient latteApi, CancellationToken cancellationToken) =>
{
    try
    {
        var response = await latteApi.GetHealthAsync(cancellationToken);
        return response.IsSuccessStatusCode
            ? Results.Ok(new { status = "ok" })
            : Results.Json(new { status = "unhealthy" }, statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch (HttpRequestException)
    {
        return Results.Json(new { status = "unhealthy" }, statusCode: StatusCodes.Status503ServiceUnavailable);
    }
})
    .WithName("GetHealth");

// MCP-REQ-004 / ADR-0002: plain REST endpoint outside the MCP tool surface. Forwards credentials
// to latteAPI's POST /auth/login and returns that response as-is.
app.MapPost("/login", async (LoginRequest request, LatteApiClient latteApi, CancellationToken cancellationToken) =>
{
    HttpResponseMessage response;
    try
    {
        response = await latteApi.LoginAsync(request, cancellationToken);
    }
    catch (HttpRequestException)
    {
        return Results.Json(new { error = "latteAPI is unreachable." }, statusCode: StatusCodes.Status502BadGateway);
    }

    var body = await response.Content.ReadAsStringAsync(cancellationToken);
    var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
    return Results.Content(body, contentType, statusCode: (int)response.StatusCode);
})
    .WithName("Login");

app.MapMcp("/mcp");

app.Run();
