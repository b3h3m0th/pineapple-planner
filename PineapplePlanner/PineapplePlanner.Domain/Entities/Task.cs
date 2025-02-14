using Google.Cloud.Firestore;
using PineapplePlanner.Domain.Enums;
using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    [FirestoreData]
    public class Task : IBaseFirestoreData
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty("Priority")]
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

        public Priority? Priority { get; set; }

        [FirestoreProperty]
        public DateTime DateDue { get; set; }

        [FirestoreProperty]
        public DateTime? CompletedAt { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? DeletedAt { get; set; }

        [FirestoreProperty]
        public DateTime? Reminder { get; set; }

        [FirestoreProperty]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [FirestoreProperty]
        public string? UserUid { get; set; }

        public Task()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

