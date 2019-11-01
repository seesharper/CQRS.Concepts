using System.Threading;
using System.Threading.Tasks;
using CQRS.Query.Abstractions;

namespace CQRS.Tests
{
    public class SampleQueryHandler : IQueryHandler<SampleQuery, SampleQueryResult>
    {
        public Task<SampleQueryResult> HandleAsync(SampleQuery query, CancellationToken cancellationToken = default)
        {
            query.WasHandled = true;
            return Task.FromResult(new SampleQueryResult());
        }
    }

    public class SampleQueryResult
    {
    }

    public class SampleQuery : IQuery<SampleQueryResult>
    {
        public bool WasHandled { get; set; }
    }
}