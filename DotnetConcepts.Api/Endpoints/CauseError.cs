namespace DotnetConcepts.Web.Endpoints;

public static class CauseErrorEndpoint
{
    public static RouteGroupBuilder MapCauseErrorEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet(
            "",
            () =>
            {
                throw new InvalidOperationException();
            }
        );

        return group;
    }
}
