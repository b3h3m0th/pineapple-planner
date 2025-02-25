using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Domain.UnitTests.Shared
{
    public class ResultBaseTests
    {
        [Fact]
        public void ResultBase_Success_ShouldSetIsSuccessTrue()
        {
            // Arrange & Act
            ResultBase result = ResultBase.Success();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
        }
    }
}
