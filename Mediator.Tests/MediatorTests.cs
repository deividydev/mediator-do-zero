using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class MediatorTests
{
    [Fact]
    public async Task SendAsync_ShouldInvokeHandlerAndReturnResponse()
    {
        // Arrange
        var request = new SampleRequest();
        var expectedResponse = "Success";

        var handlerMock = new Mock<IHandler<SampleRequest, string>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var services = new ServiceCollection();
        services.AddTransient(_ => handlerMock.Object);

        var serviceProvider = services.BuildServiceProvider();
        var mediator = new Mediator.Mediator(serviceProvider);

        // Act
        var result = await mediator.SendAsync(request);

        // Assert
        Assert.Equal(expectedResponse, result);
        handlerMock.Verify(h => h.HandleAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_ShouldThrowException_WhenHandlerNotFound()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var mediator = new Mediator.Mediator(serviceProvider);

        var request = new SampleRequest();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => mediator.SendAsync(request));
        Assert.Contains("Handler not found", exception.Message);
    }

    public class SampleRequest : IRequest<string> { }
}
