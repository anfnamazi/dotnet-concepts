namespace DotnetConcepts.Endpoints;

public static class ErrorEndpoint
{
    public static RouteGroupBuilder MapErrorEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", () => "unfortunately, an error happened!");

        return group;
    }
}
