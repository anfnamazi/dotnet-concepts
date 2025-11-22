var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGreeting();
var app = builder.Build();

app.MapGet("/", (IGreeting greeting) => greeting.GetGreeting());

app.UseLogging();

app.Run();

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
