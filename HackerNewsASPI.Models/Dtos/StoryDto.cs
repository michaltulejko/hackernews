using System.Text.Json.Serialization;

namespace HackerNewsASP.Models.Dtos;

public class StoryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("by")]
    public string By { get; set; }

    [JsonPropertyName("time")]
    public int Time { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
}
