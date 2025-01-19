using System.Text.Json.Serialization;

namespace HackerNewsASP.Tests.Models;

public class ValidationError
{
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; }
}