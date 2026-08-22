using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using latteAPI.Data;
using latteAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Generated OpenAPI document — the machine-checked source of truth for this module's HTTP
// contracts (see ADR-0005). Regenerated from these endpoint definitions on every build/run, so
// it cannot drift from the code the way a hand-maintained doc can.
builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var waitresses = builder.Configuration.GetSection("Waitresses").Get<List<WaitressAccount>>()
    ?? throw new InvalidOperationException("Configuration section 'Waitresses' is missing.");
builder.Services.AddSingleton(waitresses);

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Configuration section 'Jwt' is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<OrderStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// API-REQ-006: service health, no auth required.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("GetHealth");

// API-REQ-001: browse the menu, no auth required (API-RULE-004).
app.MapGet("/menu", () => Results.Ok(new
{
    Items = MenuCatalog.Items,
    SizeSurcharge = MenuCatalog.SizeSurcharge
}))
    .WithName("GetMenu");

// API-REQ-002: waitress login (ADR-0001). Deliberately undifferentiated 401 for
// unknown username vs. wrong password so callers can't enumerate valid usernames.
app.MapPost("/auth/login", (
    LoginRequest request,
    List<WaitressAccount> waitressAccounts,
    IOptions<JwtSettings> jwtOptions) =>
{
    var waitress = waitressAccounts.FirstOrDefault(w =>
        w.Username == request.Username && w.Password == request.Password);

    if (waitress is null)
    {
        return Results.Unauthorized();
    }

    var settings = jwtOptions.Value;
    var expiresAt = DateTimeOffset.UtcNow.AddHours(settings.ExpiryHours);

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, waitress.Username),
        new Claim("displayName", waitress.DisplayName)
    };

    var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey)),
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: settings.Issuer,
        audience: settings.Audience,
        claims: claims,
        expires: expiresAt.UtcDateTime,
        signingCredentials: signingCredentials);

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new LoginResponse(tokenString, expiresAt));
})
    .WithName("Login");

// API-REQ-003, API-RULE-001, API-RULE-002: place an order.
app.MapPost("/orders", (CreateOrderRequest request, ClaimsPrincipal user, OrderStore orderStore) =>
{
    if (request.Items is null || request.Items.Count == 0)
    {
        return Results.BadRequest(new { error = "An order must contain at least one item." });
    }

    var total = 0m;
    foreach (var line in request.Items)
    {
        var menuItem = MenuCatalog.Items.FirstOrDefault(m => m.Id == line.MenuItemId);
        if (menuItem is null)
        {
            return Results.BadRequest(new { error = $"Unknown menu item id: {line.MenuItemId}" });
        }

        total += (menuItem.BasePrice + MenuCatalog.SizeSurcharge[line.Size]) * line.Quantity;
    }

    var createdBy = user.FindFirstValue("displayName") ?? user.FindFirstValue(ClaimTypes.NameIdentifier)!;

    var order = orderStore.Add(new Order
    {
        Items = request.Items,
        Total = total,
        CreatedBy = createdBy
    });

    return Results.Created($"/orders/{order.Id}", order);
})
    .RequireAuthorization()
    .WithName("PlaceOrder");

// API-REQ-004: look up a single order.
app.MapGet("/orders/{id:guid}", (Guid id, OrderStore orderStore) =>
{
    var order = orderStore.Get(id);
    return order is null ? Results.NotFound() : Results.Ok(order);
})
    .RequireAuthorization()
    .WithName("GetOrder");

// API-REQ-005: list all orders, most recent first.
app.MapGet("/orders", (OrderStore orderStore) => Results.Ok(orderStore.GetAll()))
    .RequireAuthorization()
    .WithName("ListOrders");

app.Run();
