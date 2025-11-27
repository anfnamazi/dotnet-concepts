using DotnetConcepts.Core.Models;

namespace DotnetConcepts.Web.Endpoints;

public static class ConfigEndpoints
{
    public static RouteGroupBuilder MapConfigEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet(
            "",
            (IConfiguration configuration) =>
            {
                var config =
                    configuration.GetSection("Application").Get<Config>()
                    ?? throw new InvalidOperationException("Invalid app configuration!");

                return $"Welcome to {config.Name}'s {config.Customer}";
            }
        );
        return group;
    }
}
