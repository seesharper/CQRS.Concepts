using System;
using System.Linq;
using System.Reflection;
using CQRS.Command.Abstractions;
using CQRS.Query.Abstractions;

namespace CQRS.Execution.Abstractions
{
    public static class AssemblyExtensions
    {
        public static HandlerDescriptor[] GetCommandHandlerDescriptors(this Assembly assembly)
        {
            var commandTypes =
               assembly
                    .GetTypes()
                    .Select(t => GetGenericInterface(t, typeof(ICommandHandler<>)))
                    .Where(m => m != null);
            return commandTypes.ToArray();
        }

        public static HandlerDescriptor[] GetQueryHandlerHandlerDescriptors(this Assembly assembly)
        {
            var commandTypes =
               assembly
                    .GetTypes()
                    .Select(t => GetGenericInterface(t, typeof(IQueryHandler<,>)))
                    .Where(m => m != null);
            return commandTypes.ToArray();
        }

        private static HandlerDescriptor GetGenericInterface(Type type, Type genericTypeDefinition)
        {
            var closedGenericInterface =
                type.GetInterfaces()
                    .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);
            if (closedGenericInterface != null)
            {
                var constructor = type.GetConstructors().FirstOrDefault();
                if (constructor != null)
                {
                    var isDecorator = constructor.GetParameters().Select(p => p.ParameterType)
                        .Contains(closedGenericInterface);
                    if (!isDecorator) return new HandlerDescriptor(closedGenericInterface, type);
                }
            }
            return null;
        }

    }

    public class HandlerDescriptor
    {
        public HandlerDescriptor(Type handlerType, Type implementingType)
        {
            HandlerType = handlerType;
            ImplementingType = implementingType;
        }

        public Type HandlerType { get; }
        public Type ImplementingType { get; }
    }
}