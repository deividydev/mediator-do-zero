namespace Mediator.Abstractions;

/// <summary>
/// Represents a request that expects a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned after processing the request.</typeparam>
public interface IRequest<TResponse>;