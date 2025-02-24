using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Dto.Gemini
{
    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }

        [JsonPropertyName("finishReason")]
        public string? FinishReason { get; set; }

        [JsonPropertyName("avgLogprobs")]
        public float AvgLogprobs { get; set; }
    }
}
