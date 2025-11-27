using DotnetConcepts.Models;

namespace DotnetConcepts.Endpoints;

public static class PostEndpoints
{
    public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet(
            "",
            async (IHttpClientFactory httpClientFactory, HttpContext httpContext) =>
            {
                var client = httpClientFactory.CreateClient("Posts");
                var response = await client.GetAsync("/posts?_limit=10");
                var posts = await response.Content.ReadFromJsonAsync<Post[]>();

                httpContext.Response.ContentType = "text/html";
                var postsHtml = string.Join(
                    "",
                    posts?.Select(p => $"<li>{p.Title}<br/>{p.Body}</li>") ?? [""]
                );
                return $"<h2>Top 10 posts:</h2><ul>{postsHtml}</ul>";
            }
        );
        return group;
    }
}
