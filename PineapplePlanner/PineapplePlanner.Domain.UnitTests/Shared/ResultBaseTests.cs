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

        [Fact]
        public void ResultBase_AddErrorAndSetFailure_ShouldSetIsSuccessFalse()
        {
            ResultBase result = ResultBase.Success();

            result.AddErrorAndSetFailure("Test error");

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void ResultBase_DefaultConstructor_ShouldInitializeEmptyErrors()
        {
            ResultBase result = new ResultBase();

            Assert.Empty(result.Errors);
        }
    }

    public class ResultBaseTTests
    {
        [Fact]
        public void ResultBaseT_Success_ShouldSetIsSuccessTrue()
        {
            ResultBase<int> result = ResultBase<int>.Success();

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ResultBaseT_Success_ShouldContainNoErrors()
        {
            ResultBase<int> result = ResultBase<int>.Success();

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ResultBaseT_SuccessWithData_ShouldSetData()
        {
            string testData = "test";
            ResultBase<string> result = ResultBase<string>.Success(testData);

            Assert.Equal(testData, result.Data);
        }

        [Fact]
        public void ResultBaseT_SuccessWithData_ShouldSetIsSuccessTrue()
        {
            string testData = "test";
            ResultBase<string> result = ResultBase<string>.Success(testData);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ResultBaseT_Failure_ShouldSetIsSuccessFalse()
        {
            ResultBase<int> result = ResultBase<int>.Failure();

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void ResultBaseT_Failure_ShouldContainNoErrors()
        {
            ResultBase<int> result = ResultBase<int>.Failure();

            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ResultBaseT_ConstructorWithData_ShouldSetData()
        {
            int testData = 42;
            ResultBase<int> result = new ResultBase<int>(testData);

            Assert.Equal(testData, result.Data);
        }

        [Fact]
        public void ResultBaseT_AddErrorAndSetFailure_ShouldPreserveData()
        {
            int testData = 42;
            ResultBase<int> result = ResultBase<int>.Success(testData);

            result.AddErrorAndSetFailure("Test error");

            Assert.Equal(testData, result.Data);
        }
    }
}