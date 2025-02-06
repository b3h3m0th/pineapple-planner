using PineapplePlanner.Domain.Interfaces;

namespace PineapplePlanner.Domain.Entities
{
    public class Tag : IBaseFirestoreData
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}

