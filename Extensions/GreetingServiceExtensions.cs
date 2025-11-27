using DotnetConcepts.Services;

namespace DotnetConcepts.Extensions;

public static class GreetingServiceExtensions
{
    public static IServiceCollection AddGreeting(this IServiceCollection services)
    {
        services.AddScoped<IGreeting, Greeting>();
        return services;
    }
}
