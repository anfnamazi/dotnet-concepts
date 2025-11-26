using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddGreeting();

// Health Checks
// builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks().AddCheck<CustomHealthCheck>("CustomHealthCheck");

// Outgoing HTTP Request
builder.Services.AddHttpClient();
builder.Services.AddHttpClient(
    "Posts",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
    }
);

var app = builder.Build();

// Dependency Injection
app.MapGet("/", (IGreeting greeting) => greeting.GetGreeting());

// Middleware
app.UseLogging();

// Configuration
app.MapGet(
    "/welcome",
    (IConfiguration configuration) =>
    {
        var config =
            configuration.GetSection("Application").Get<Config>()
            ?? throw new InvalidOperationException("Invalid app configuration!");

        return $"Welcome to {config.Name}'s {config.Customer}";
    }
);

// Logging
app.MapGet(
    "/log",
    (ILogger<Program> logger) =>
    {
        logger.LogInformation("information logging...");
        logger.LogDebug("debug logging...");
        logger.LogWarning("warning logging...");
        logger.LogCritical("critical logging...");
    }
);

// Health Checks
app.MapHealthChecks("/health");

// HTTP Context
app.MapGet(
    "/http-context",
    (HttpContext context) =>
    {
        context.Response.StatusCode = 201;
        context.Response.ContentType = "text/html";

        return $"<h2>{context.Request.Path.Value}</h2><p>This is a test page!</p>";
    }
);

// Dynamic Routing
app.MapGet("/dynamic-route/{name?}", (string? name) => $"Hello World to {name}");

// Static File
app.UseStaticFiles();

/* #region Error Handling */
app.MapGet(
    "cause-error",
    () =>
    {
        throw new InvalidOperationException();
    }
);
app.MapGet("/error", () => "unfortunately, an error happened!");
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

/* #endregion */

/* #region Outgoing HTTP Request */
app.MapGet(
    "/posts",
    async (IHttpClientFactory httpClientFactory, HttpContext httpContext) =>
    {
        var httpClient = httpClientFactory.CreateClient("Posts");
        var response = await httpClient.GetAsync("posts?_limit=10");
        var posts = await response.Content.ReadFromJsonAsync<Post[]>();

        httpContext.Response.ContentType = "text/html";

        var postsHtml = string.Join(
            "",
            posts?.Select(j => $"<li>{j.Title}<br/>{j.Body}</li>") ?? [""]
        );
        return $"<h2>Top 10 posts:</h2><ul>{postsHtml}</ul>";
    }
);

/* #endregion */

app.Run();

/* #region Dependency Injection */
interface IGreeting
{
    public string GetGreeting();
}

class Greeting : IGreeting
{
    public string GetGreeting()
    {
        return "Hello!";
    }
}

public static class GreetingServiceBuilderExtension
{
    public static IServiceCollection AddGreeting(this IServiceCollection services)
    {
        services.AddScoped<IGreeting, Greeting>();

        return services;
    }
}

/* #endregion */

/* #region Middleware */
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("my-header", "my-value");

        await _next(context);

        Console.WriteLine(context.Request.Path);
    }
}

public static class LoggingMiddlewareExtension
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}

/* #endregion */

/* #region Configuration */
public record Config(string Name, string Customer);

/* #endregion */

/* #region Environment */
// app.Environment.IsDevelopment() | for Development;
// app.Environment.IsProduction() | For live system;
// app.Environment.IsStaging() | For Testing
/* #endregion */

/* #region Health Checks */
public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy));
    }
}

/* #endregion */

/* #region Outgoing HTTP Request */
public record Post
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonPropertyName("userId")]
    public required int UserId { get; set; }
}
/* #endregion */
