using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.BlockedCustomerConsumers;
using QUserService.Contracts.Events.BlockedCustomerEvent;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.BlockedCustomerConsumerTests;

public class BlockedCustomerCreatedConsumerTests
{
    private readonly Mock<ILogger<BlockedCustomerCreatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<BlockedCustomerCreatedEvent>> _mockContext;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly BlockedCustomerCreatedConsumer _consumer;

    public BlockedCustomerCreatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<BlockedCustomerCreatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<BlockedCustomerCreatedEvent>>();
        _mockCacheService = new Mock<ICacheService>();
        _consumer = new BlockedCustomerCreatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_BlockedCustomer_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new BlockedCustomerCreatedEvent
        {
            CompanyId = 1,
            CustomerId = 1,
            BlockedCustomerId = 1,
            DoesBanForever = false,
            BannedUntil = DateTime.UtcNow.Date.AddMonths(1),
            Reason = null,
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