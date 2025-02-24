using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Enums;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.JsonConverter;
using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Task : IBaseFirestoreData
    {
        [FirestoreProperty]
        [JsonPropertyName("Id")]
        public required string Id { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("Name")]
        public required string Name { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [FirestoreProperty("Priority")]
        [JsonIgnore]
        public string? PriorityString
        {
            private get => Priority?.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value) && Enum.TryParse<Priority>(value, true, out var parsedPriority))
                {
                    Priority = parsedPriority;
                }
                else
                {
                    Priority = null;
                }
            }
        }

        [JsonConverter(typeof(PriorityEnumConverter))]
        public Priority? Priority { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("DateDue")]
        public DateTime? DateDue { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("CompletedAt")]
        public DateTime? CompletedAt { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("DeletedAt")]
        public DateTime? DeletedAt { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("Reminder")]
        public DateTime? Reminder { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("StartDate")]
        public DateTime? StartDate { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("EndDate")]
        public DateTime? EndDate { get; set; }

        [FirestoreProperty]
        [JsonIgnore]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [FirestoreProperty]
        [JsonPropertyName("UserUid")]
        public string? UserUid { get; set; }
    }
}

