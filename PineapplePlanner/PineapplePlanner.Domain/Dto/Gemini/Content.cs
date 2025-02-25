using PineapplePlanner.Domain.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Dto.Gemini
{
    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part>? Parts { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        public ResultBase<Entities.Task> GetTask()
        {
            ResultBase<Entities.Task> result = ResultBase<Entities.Task>.Success();

            if (Parts != null && Parts.Count > 0 && !string.IsNullOrEmpty(Parts[0].Text))
            {
                try
                {
                    Entities.Task? task = JsonSerializer.Deserialize<Entities.Task>(Parts[0].Text!, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });

                    result.Data = task;
                }
                catch (JsonException ex)
                {
                    result.AddErrorAndSetFailure(ex.Message);
                }
            }

            return result;
        }
    }
}
