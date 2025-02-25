using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Domain.UnitTests.Shared
{
    public class ResultBaseTests
    {
        [Fact]
        public void ResultBase_Success_ShouldSetIsSuccessTrue()
        {
            ResultBase result = ResultBase.Success();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ResultBase_Success_ShouldContainNoErrors()
        {
            ResultBase result = ResultBase.Success();

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ResultBase_Failure_ShouldSetIsSuccessFalse()
        {
            ResultBase result = ResultBase.Failure();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void ResultBase_Failure_ShouldContainNoErrors()
        {
            ResultBase result = ResultBase.Failure();

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ResultBase_AddErrorAndSetFailure_ShouldAddError()
        {
            ResultBase result = ResultBase.Success();
            string errorMessage = "Test error";

            result.AddErrorAndSetFailure(errorMessage);

            Assert.NotEmpty(result.Errors);
            Assert.Contains(errorMessage, result.Errors);
        }
    }
}