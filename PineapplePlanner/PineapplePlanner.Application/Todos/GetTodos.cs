using MediatR;
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
            public List<Domain.Entities.Entry>? Todos { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                Result result = new Result();

                try
                {
                    await System.Threading.Tasks.Task.Delay(500);

                    result.Todos = new List<Domain.Entities.Entry>()
                    {
                        new Domain.Entities.Entry()
                        {
                            Id = "1",
                            Name = "Todo1"
                        },
                        new Domain.Entities.Entry()
                        {
                            Id = "2",
                            Name = "Todo2"
                        }
                    };
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
