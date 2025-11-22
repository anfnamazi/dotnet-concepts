var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGreeting();
var app = builder.Build();

app.MapGet("/", (IGreeting greeting) => greeting.GetGreeting());

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
