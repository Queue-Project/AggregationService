using BranchService.Contracts.Events.BranchEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.BranchConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.BranchConsumerTests;

public class BranchCreatedConsumerTests
{
    private readonly Mock<ILogger<BranchCreatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<BranchCreatedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly BranchCreatedConsumer _consumer;

    public BranchCreatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<BranchCreatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<BranchCreatedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new BranchCreatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Branch_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new BranchCreatedEvent()
        {
            CompanyId = 1,
            BranchId = 1,
            BranchName = "Test Branch Name",
            EmailAddress = "test@gmail.com",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            IsActive = true,
            OccuredAt = DateTime.UtcNow
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.Remove(It.IsAny<string>()), Times.AtLeast(2));
    }

  
}