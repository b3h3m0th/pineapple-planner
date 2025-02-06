namespace PineapplePlanner.Domain.Shared
{
    public class ResultBase
    {
        public bool IsSuccess { get; set; }

        public ResultBase() { }

        public static ResultBase Success() => new ResultBase() { IsSuccess = true };
        public static ResultBase Failure() => new ResultBase() { IsSuccess = false };
    }

    public class ResultBase<T> : ResultBase
    {
        public T? Data { get; set; }

        public ResultBase() { }

        public ResultBase(T data)
        {
            Data = data;
        }

        public static ResultBase<T> Success(T data)
        {
            return new ResultBase<T>()
            {
                IsSuccess = true,
                Data = data
            };
        }

        new public static ResultBase<T> Failure() => new ResultBase<T>() { IsSuccess = false };
    }
}
