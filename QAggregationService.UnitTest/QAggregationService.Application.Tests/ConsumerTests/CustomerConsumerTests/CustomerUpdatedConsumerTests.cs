using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CustomerConsumers;
using QAuthService.Contracts.Events.CustomerEvent;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CustomerConsumerTests;

public class CustomerUpdatedConsumerTests
{
    private readonly Mock<ILogger<CustomerUpdatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<CustomerUpdatedEvent>> _mockContext;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly CustomerUpdatedConsumer _consumer;

    public CustomerUpdatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CustomerUpdatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<CustomerUpdatedEvent>>();
        _mockCacheService = new Mock<ICacheService>();
        _consumer = new CustomerUpdatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_BlockedCustomer_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new CustomerUpdatedEvent
        {
            CustomerId = 1,
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            PhoneNumber = "+992923324252",
            OccuredAt = DateTime.UtcNow
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.RemoveAsync(It.IsAny<string>()), Times.AtLeast(2));
    }

    [Fact]
    public async Task Consume_Should_Throw_When_Event_Did_Not_Received()
    {
        //Arrange
        var expectedEvent = new Exception("Event did not received");


        _mockContext.Setup(s => s.Message).Throws(expectedEvent);


        //Act
        var result = _consumer.Consume(_mockContext.Object);

        //Assert

        var exception = await result.ShouldThrowAsync<Exception>();
        exception.Message.ShouldBe(expectedEvent.Message);
        
        _mockCacheService.Verify(s => s.RemoveAsync(It.IsAny<string>()), Times.Never);
    }
}