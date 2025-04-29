using Mediator.Abstractions;
using Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Entry point to test the Mediator implementation with a sample request and handler.
/// </summary>
var services = new ServiceCollection();

services.AddMediator(typeof(Program).Assembly);
services.AddTransient<UserRepository>();

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

var request = new CreateUserRequest("Mars");
var result = await mediator.SendAsync(request);

Console.WriteLine(result);

/// <summary>
/// Represents a simple repository for saving user data.
/// </summary>
public class UserRepository
{
    /// <summary>
    /// Simulates saving a user to the database.
    /// </summary>
    public void Save() => Console.WriteLine("Saving...");
}

/// <summary>
/// Represents a request to create a new user.
/// </summary>
/// <param name="name">The name of the user to be created.</param>
public class CreateUserRequest(string name) : IRequest<string>
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; } = name;
}

/// <summary>
/// Handles the <see cref="CreateUserRequest"/> and returns a confirmation message.
/// </summary>
/// <param name="userRepository">The repository used to save the user data.</param>
public class CreateUserHandler(UserRepository userRepository) : IHandler<CreateUserRequest, string>
{
    /// <summary>
    /// Handles the creation of a new user asynchronously.
    /// </summary>
    /// <param name="request">The request containing user information.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task containing the result message.</returns>
    public Task<string> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Creating {request.Name} user...");
        userRepository.Save();
        return Task.FromResult($"{request.Name} user created.");
    }
}
