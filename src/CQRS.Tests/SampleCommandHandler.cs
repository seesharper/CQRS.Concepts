using System.Threading;
using System.Threading.Tasks;
using CQRS.Command.Abstractions;

namespace CQRS.Tests
{
    public class SampleCommandHandler : ICommandHandler<SampleCommand>
    {
        public Task HandleAsync(SampleCommand command, CancellationToken cancellationToken = default)
        {
            command.WasHandled = true;
            return Task.CompletedTask;
        }
    }


    public class SampleCommand
    {
        public bool WasHandled { get; set; }
    }
}