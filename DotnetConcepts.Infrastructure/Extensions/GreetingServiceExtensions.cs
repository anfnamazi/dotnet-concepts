using DotnetConcepts.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetConcepts.Infrastructure.Extensions;

public static class GreetingServiceExtensions
{
    public static IServiceCollection AddGreeting(this IServiceCollection services)
    {
        services.AddScoped<IGreeting, Greeting>();
        return services;
    }
}
