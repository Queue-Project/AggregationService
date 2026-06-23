using BranchService.Contracts.Events.BranchEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.BranchConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.BranchConsumerTests;

public class BranchDeletedConsumerTests
{
    
    private readonly Mock<ILogger<BranchDeletedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<BranchDeletedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly BranchDeletedConsumer _consumer;

    public BranchDeletedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<BranchDeletedConsumer>>();
        _mockContext = new Mock<ConsumeContext<BranchDeletedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new BranchDeletedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Branch_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new BranchDeletedEvent()
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