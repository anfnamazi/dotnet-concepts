using System.Text.Json.Serialization;

namespace DotnetConcepts.Core.Models;

public record Post
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonPropertyName("userId")]
    public required int UserId { get; set; }
}
