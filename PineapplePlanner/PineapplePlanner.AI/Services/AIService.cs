using Microsoft.Extensions.Configuration;
using PineapplePlanner.Domain.Dto.Gemini;
using PineapplePlanner.Domain.Enums;
using PineapplePlanner.Domain.Shared;
using System.Text;
using System.Text.Json;

namespace PineapplePlanner.AI.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AIService(IConfiguration configuration)
        {
            _configuration = configuration;
            string apiKey = _configuration["google-generative-ai-api-key"] ?? string.Empty;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}"),
            };
        }

        public async Task<ResultBase<Domain.Entities.Entry>> GenerateTaskFromPrompt(string prompt)
        {
            ResultBase<Domain.Entities.Entry> result = ResultBase<Domain.Entities.Entry>.Success();

            DateTime now = DateTime.Now;

            try
            {
                var requestBody = new
                {
                    system_instruction = new
                    {
                        parts = new
                        {
                            text = $"""
                            You are a task management assistant.
                            Use this date-time as the reference point for calculating dates like tomorrow or next week: {now:s}.
                            Use this date-time as CreatedAt date: {now:s}.
                            Always keep the required structure of your response.
                            """,
                        }
                    },
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        response_mime_type = "application/json",
                        response_schema = new
                        {
                            type = "OBJECT",
                            properties = new
                            {
                                Id = new { type = "STRING" },
                                Name = new { type = "STRING" },
                                Description = new { type = "STRING" },
                                Priority = new { type = "STRING", @enum = new[] { nameof(Priority.Low), nameof(Priority.Medium), nameof(Priority.High) } },
                                DateDue = new { type = "STRING", format = "date-time" },
                                Tags = new { type = "ARRAY", items = new { type = "STRING" } },
                            },
                            required = new[] { "Id", "Name", "Priority", "DateDue", "Tags" }
                        }
                    }
                };

                string json = JsonSerializer.Serialize(requestBody);
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("", content);
                string responseString = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
                };
                GeminiResponseDto? geminiResponseDto = JsonSerializer.Deserialize<GeminiResponseDto>(responseString, options);

                ResultBase<EntryDto>? taskDtoResult = geminiResponseDto?.GetFirstTaskDto();

                if (taskDtoResult?.Data == null)
                {
                    result.AddErrorAndSetFailure("Something went wrong");
                }
                else
                {
                    EntryDto taskDto = taskDtoResult.Data;
                    result.Data = new Domain.Entities.Task()
                    {
                        Id = taskDto.Id,
                        Name = taskDto.Name,
                        Description = taskDto.Description,
                        Priority = taskDto.Priority,
                        DateDue = taskDto.DateDue,
                        CreatedAt = DateTime.Now,
                        Tags = [.. taskDto.Tags.Select(t => new Domain.Entities.Tag()
                        {
                            Id = string.Empty,
                            Name = t.ToLower()
                        })]
                    };

                    foreach (string error in taskDtoResult.Errors)
                    {
                        result.AddErrorAndSetFailure(error);
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }
    }
}
