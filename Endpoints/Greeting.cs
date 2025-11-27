using DotnetConcepts.Services;

namespace DotnetConcepts.Endpoints;

public static class GreetingEndpoints
{
    public static RouteGroupBuilder MapGreetingEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", (IGreeting greeting) => greeting.GetGreeting());

        return group;
    }
}
