using System;
using System.Threading;
using System.Threading.Tasks;
using CQRS.Command.Abstractions;

namespace CQRS.Execution.Abstractions
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandExecutor(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        {
            var commandHandler = commandHandlerFactory.CreateCommandHandler<TCommand>();
            await commandHandler.HandleAsync(command, cancellationToken).ConfigureAwait(false);
        }
    }

    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateCommandHandler<TCommand>();
    }
}
