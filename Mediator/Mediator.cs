using Mediator.Abstractions;

namespace Mediator;

/// <summary>
/// Default implementation of the <see cref="IMediator"/> interface, responsible for dispatching requests to their corresponding handlers.
/// </summary>
/// <param name="_serviceProvider">The service provider used to resolve handlers.</param>
public class Mediator(IServiceProvider _serviceProvider) : IMediator
{
    /// <summary>
    /// Sends a request to its corresponding handler and returns the response asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
    /// <param name="request">The request object to be handled.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the handler or its method could not be found, or if the method returns an unexpected type.
    /// </exception>
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
                                                      CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handleType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetService(handleType)
            ?? throw new InvalidOperationException($"Handler not found for {requestType}");

        var method = handleType.GetMethod("HandleAsync")
            ?? throw new InvalidOperationException($"Method not found for {requestType}");

        var result = method.Invoke(handler, [request, cancellationToken]);
        if (result is not Task<TResponse> task)
            throw new InvalidOperationException($"Method returned unexpected type {result}");

        return await task;
    }
}