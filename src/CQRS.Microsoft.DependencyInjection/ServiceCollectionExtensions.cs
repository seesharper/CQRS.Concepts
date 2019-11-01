using System;
using System.Linq;
using System.Reflection;
using CQRS.Command.Abstractions;
using CQRS.Execution.Abstractions;
using CQRS.Query.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Microsoft.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            return AddCommandHandlers(serviceCollection, Assembly.GetCallingAssembly());
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var commandHanderDescriptions = assembly.GetCommandHandlerDescriptors();

            foreach (var commandHanderDescription in commandHanderDescriptions)
            {
                serviceCollection.AddScoped(commandHanderDescription.HandlerType, commandHanderDescription.ImplementingType);
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(ICommandHandlerFactory)))
            {
                serviceCollection.AddScoped<ICommandHandlerFactory>(sp => new CommandHandlerFactory(sp));
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(ICommandExecutor)))
            {
                serviceCollection.AddScoped<ICommandExecutor, CommandExecutor>();
            }

            return serviceCollection;
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection)
        {
            return AddQueryHandlers(serviceCollection, Assembly.GetCallingAssembly());
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var queryHandlerDescriptions = assembly.GetQueryHandlerHandlerDescriptors();

            foreach (var queryHandlerDescription in queryHandlerDescriptions)
            {
                serviceCollection.AddScoped(queryHandlerDescription.HandlerType, queryHandlerDescription.ImplementingType);
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IQueryHandlerFactory)))
            {
                serviceCollection.AddScoped<IQueryHandlerFactory>(sp => new QueryHandlerFactory(sp));
            }

            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IQueryExecutor)))
            {
                serviceCollection.AddScoped<IQueryExecutor, QueryExecutor>();
            }

            return serviceCollection;
        }
    }

    internal class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICommandHandler<TCommand> CreateCommandHandler<TCommand>()
        {
            return serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        }
    }

    internal class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public QueryHandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetQueryHandler(Type queryHandlerType)
        {
            return serviceProvider.GetRequiredService(queryHandlerType);
        }
    }
}
