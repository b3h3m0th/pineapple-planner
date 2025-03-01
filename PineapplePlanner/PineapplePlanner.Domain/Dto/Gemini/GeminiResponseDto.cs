using PineapplePlanner.Domain.Shared;
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

        public ResultBase<TaskDto> GetFirstTaskDto()
        {
            ResultBase<TaskDto> result = ResultBase<TaskDto>.Success();
            ResultBase<TaskDto>? taskResult = Candidates?[0]?.Content?.GetTaskDto();

            if (taskResult != null)
            {
                result.Data = taskResult.Data;
                foreach (string error in taskResult.Errors)
                {
                    result.AddErrorAndSetFailure(error);
                }
            }
            else
            {
                result.AddErrorAndSetFailure("Something went wrong");
            }

            return result;
        }
    }
}
