using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Dto.Gemini
{
    public class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
