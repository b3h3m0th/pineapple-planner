using PineapplePlanner.Domain.Enums;

namespace PineapplePlanner.Domain.UnitTests.Entities
{
    public class EntryTests
    {
        [Theory]
        [InlineData("High", Priority.High)]
        [InlineData("high", Priority.High)]
        [InlineData("Medium", Priority.Medium)]
        [InlineData("medium", Priority.Medium)]
        [InlineData("Low", Priority.Low)]
        [InlineData("low", Priority.Low)]
        [InlineData("InvalidPriority", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void PriorityString_ConvertsToPriorityCorrectly(string input, Priority? expectedPriority)
        {
            Domain.Entities.Entry entry = new Domain.Entities.Entry()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Entry",
                Priority = Priority.Medium
            };

            entry.PriorityString = input;

            Assert.Equal(expectedPriority, entry.Priority);
        }
    }
}
