namespace PineapplePlanner.Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime? Reminder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
