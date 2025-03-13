using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Enums;
using PineapplePlanner.Domain.Interfaces;
using PineapplePlanner.Domain.JsonConverter;
using System.Text.Json.Serialization;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Entry : IBaseFirestoreData
    {
        [FirestoreProperty]
        public string Type { get; } = nameof(Entry);

        [FirestoreProperty]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty("Priority")]
        public string? PriorityString
        {
            protected get => Priority?.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value) && Enum.TryParse<Priority>(value, true, out Priority parsedPriority))
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
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? DeletedAt { get; set; }

        [FirestoreProperty]
        public List<Tag> Tags { get; set; } = [];

        [FirestoreProperty]
        public string? UserUid { get; set; }

        public Entry() { }

        protected Entry(string type)
        {
            Type = type;
        }
    }
}

