using DotnetConcepts.Middleware;
using Microsoft.AspNetCore.Builder;

namespace DotnetConcepts.Infrastructure.Extensions;

public static class LoggingMiddlewareExtension
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
