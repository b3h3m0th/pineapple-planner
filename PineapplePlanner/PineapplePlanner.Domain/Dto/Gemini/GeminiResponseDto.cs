using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Dto.Gemini
{
    public class GeminiResponseDto
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }

        [JsonPropertyName("usageMetadata")]
        public UsageMetadata? UsageMetadata { get; set; }

        [JsonPropertyName("modelVersion")]
        public string? ModelVersion { get; set; }

        public Entities.Task? GetFirstTask()
        {
            if (Candidates != null && Candidates.Count > 0)
            {
                return Candidates?[0]?.Content?.GetTask().Data;
            }

            return null;
        }
    }
}
