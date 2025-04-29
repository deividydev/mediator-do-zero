namespace Mediator.Abstractions;

/// <summary>
/// Defines a mediator that is responsible for sending requests to their corresponding handlers.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request to its handler and returns a response asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
    /// <param name="request">The request object to be handled.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response.</returns>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}