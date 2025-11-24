var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGreeting();
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
