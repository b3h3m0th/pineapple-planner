using MediatR;
using PineapplePlanner.Domain.Entities;
using System.Diagnostics;

namespace PineapplePlanner.Application
{
    /// <summary>
    /// Returns all employees from the database
    /// </summary>
    public class GetTodos
    {
        public record Query() : IRequest<Result>;

        public class Result
        {
            public List<Todo> Todos { get; set; } = new List<Todo>();
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                Result result = new Result();

                try
                {
                    await Task.Delay(500);

                    return result;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }

                return result;
            }
        }
    }
}
