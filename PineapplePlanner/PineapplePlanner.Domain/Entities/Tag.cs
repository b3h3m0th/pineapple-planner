namespace PineapplePlanner.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public required string Name { get; set; };
        public string Color { get; set; } = string.Empty;
    }
}

