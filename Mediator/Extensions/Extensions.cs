using System.Reflection;
using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Extensions;

/// <summary>
/// Extension methods for configuring the Mediator in the service collection.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds the Mediator and automatically registers all handlers found in the provided assemblies.
    /// </summary>
    /// <param name="services">The service collection where the Mediator and its handlers will be registered.</param>
    /// <param name="assemblies">Assemblies to search for handlers that implement <see cref="IHandler{TRequest, TResponse}"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient<IMediator, Mediator>();

        var handlerType = typeof(IHandler<,>);

        foreach (var assembly in assemblies)
        {
            var handlers = assembly.GetTypes()
                                   .Where(t => !t.IsInterface && !t.IsAbstract)
                                   .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                                   .Where(ti => ti.Interface.IsGenericType &&
                                                ti.Interface.GetGenericTypeDefinition().Equals(handlerType));

            foreach (var handler in handlers)
                services.AddTransient(handler.Interface, handler.Type);
        }

        return services;
    }
}
