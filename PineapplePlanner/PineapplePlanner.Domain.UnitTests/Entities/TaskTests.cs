using PineapplePlanner.Domain.Enums;

namespace PineapplePlanner.Domain.UnitTests.Entities
{
    public class TaskTests
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
            Domain.Entities.Task task = new Domain.Entities.Task()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Task",
                Priority = Priority.Medium
            };

            task.PriorityString = input;

            Assert.Equal(expectedPriority, task.Priority);
        }
    }
}
