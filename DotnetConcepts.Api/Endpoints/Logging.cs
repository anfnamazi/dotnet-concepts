namespace DotnetConcepts.Web.Endpoints;

public static class LoggingEndpoints
{
    public static RouteGroupBuilder MapLoggingEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet(
            "",
            (ILogger<Program> logger) =>
            {
                logger.LogInformation("information logging...");
                logger.LogDebug("debug logging...");
                logger.LogWarning("warning logging...");
                logger.LogCritical("critical logging...");
            }
        );
        return group;
    }
}
