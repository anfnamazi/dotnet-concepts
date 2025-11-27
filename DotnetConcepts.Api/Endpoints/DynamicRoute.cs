namespace DotnetConcepts.Web.Endpoints;

public static class DynamicRouteEndpoints
{
    public static RouteGroupBuilder MapDynamicRouteEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/{name?}", (string? name) => $"Hello World to {name}");

        return group;
    }
}
