namespace DotnetConcepts.Endpoints;

public static class HttpContextEndpoints
{
    public static RouteGroupBuilder MapHttpContextEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet(
            "",
            (HttpContext context) =>
            {
                context.Response.StatusCode = 201;
                context.Response.ContentType = "text/html";

                return $"<h2>{context.Request.Path.Value}</h2><p>This is a test page!</p>";
            }
        );
        return group;
    }
}
