using Microsoft.Extensions.Configuration;
using PineapplePlanner.Domain.Dto.Gemini;
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

        public async Task<ResultBase<Domain.Entities.Task>> GenerateTaskFromPrompt(string prompt)
        {
            ResultBase<Domain.Entities.Task> result = ResultBase<Domain.Entities.Task>.Success();

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
                                Priority = new { type = "STRING", @enum = new[] { "Low", "Medium", "High" } },
                                DateDue = new { type = "STRING", format = "date-time" },
                                CreatedAt = new { type = "STRING", format = "date-time" },
                                Tags = new { type = "ARRAY", items = new { type = "STRING" } },
                            },
                            required = new[] { "Id", "Name", "Priority", "DateDue", "Tags" }
                        }
                    }
                };

                string json = JsonSerializer.Serialize(requestBody);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("", content);
                string responseString = await response.Content.ReadAsStringAsync();

                GeminiResponseDto? geminiResponseDto = JsonSerializer.Deserialize<GeminiResponseDto>(responseString, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
                });

                ResultBase<TaskDto>? taskDtoResult = geminiResponseDto?.GetFirstTaskDto();

                if (taskDtoResult?.Data == null)
                {
                    result.AddErrorAndSetFailure("Something went wrong");
                }
                else
                {
                    TaskDto taskDto = taskDtoResult.Data;
                    result.Data = new Domain.Entities.Task()
                    {
                        Id = taskDto.Id,
                        Name = taskDto.Name,
                        Description = taskDto.Description,
                        Priority = taskDto.Priority,
                        DateDue = taskDto.DateDue,
                        StartDate = taskDto.StartDate,
                        EndDate = taskDto.EndDate,
                        Tags = taskDto.Tags.Select(t => new Domain.Entities.Tag()
                        {
                            Id = string.Empty,
                            Name = t
                        }).ToList()
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
