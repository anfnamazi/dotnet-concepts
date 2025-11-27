using DotnetConcepts.Infrastructure.Extensions;
using DotnetConcepts.Infrastructure.HealthChecks;
using DotnetConcepts.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddGreeting();
builder.Services.AddHealthChecks().AddCheck<CustomHealthCheck>("CustomHealthCheck");
builder.Services.AddHttpClient();
builder.Services.AddHttpClient(
    "Posts",
    client =>
    {
        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
    }
);

var app = builder.Build();

// Middleware
app.UseLogging();
app.UseStaticFiles();

// Endpoints
app.MapGroup("/").MapGreetingEndpoints();
app.MapGroup("/error").MapErrorEndpoints();
app.MapGroup("/welcome").MapConfigEndpoints();
app.MapGroup("/log").MapLoggingEndpoints();
app.MapGroup("/health").MapHealthEndpoints();
app.MapGroup("/http-context").MapHttpContextEndpoints();
app.MapGroup("/dynamic-route").MapDynamicRouteEndpoints();
app.MapGroup("/posts").MapPostEndpoints();
app.MapGroup("/cause-error").MapCauseErrorEndpoints();

app.Run();
