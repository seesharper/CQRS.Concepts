using System.Threading.Tasks;
using CQRS.Command.Abstractions;
using CQRS.Microsoft.DependencyInjection;
using CQRS.Query.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CQRS.Tests
{
    public class CommandExecutorTests
    {
        [Fact]
        public async Task ShouldExecuteCommandHandler()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandHandlers();

            var provider = serviceCollection.BuildServiceProvider();
            var commandExecutor = provider.GetRequiredService<ICommandExecutor>();

            var command = new SampleCommand();
            await commandExecutor.ExecuteAsync(command);

            command.WasHandled.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldExecuteQueryHandler()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddQueryHandlers();
            var provider = serviceCollection.BuildServiceProvider();

            var queryExecutor = provider.GetRequiredService<IQueryExecutor>();

            var query = new SampleQuery();
            var result = await queryExecutor.ExecuteAsync(query);

            query.WasHandled.Should().BeTrue();
        }
    }
}
