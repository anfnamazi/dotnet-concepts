using DotnetConcepts.Core.Services;

namespace DotnetConcepts.Web.Endpoints;

public static class GreetingEndpoints
{
    public static RouteGroupBuilder MapGreetingEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", (IGreeting greeting) => greeting.GetGreeting());

        return group;
    }
}
