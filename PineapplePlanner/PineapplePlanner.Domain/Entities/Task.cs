namespace PineapplePlanner.Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? Reminder { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
