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

        public ResultBase<TaskDto> GetTaskDto()
        {
            ResultBase<TaskDto> result = ResultBase<TaskDto>.Success();

            if (Parts != null && Parts.Count > 0 && !string.IsNullOrEmpty(Parts[0].Text))
            {
                try
                {
                    TaskDto? taskDto = JsonSerializer.Deserialize<TaskDto>(Parts[0].Text!, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                    });

                    result.Data = taskDto;
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
