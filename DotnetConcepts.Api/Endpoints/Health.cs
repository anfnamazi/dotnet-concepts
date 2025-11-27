namespace DotnetConcepts.Web.Endpoints;

public static class HealthEndpoint
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapHealthChecks("");

        return group;
    }
}
