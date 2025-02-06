using PineapplePlanner.Domain.Enums;

namespace PineapplePlanner.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Priority? Priority { get; set; }
        public DateTime Date { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? Reminder { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}

