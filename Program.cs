using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddGreeting();

// Health Checks
// builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks().AddCheck<CustomHealthCheck>("CustomHealthCheck");

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

// Error Handling
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
