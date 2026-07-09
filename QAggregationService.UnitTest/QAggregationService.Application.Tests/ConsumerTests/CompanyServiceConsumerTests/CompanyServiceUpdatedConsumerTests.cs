using BranchService.Contracts.Events.CompanyServiceEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CompanyServiceConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CompanyServiceConsumerTests;

public class CompanyServiceUpdatedConsumerTests
{
    private readonly Mock<ILogger<CompanyServiceUpdatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<CompanyServiceUpdatedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly CompanyServiceUpdatedConsumer _consumer;

    public CompanyServiceUpdatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CompanyServiceUpdatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<CompanyServiceUpdatedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new CompanyServiceUpdatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Branch_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new CompanyServiceUpdatedEvent()
        {
            CompanyId = 1,
            CompanyServiceId = 1,
            ServiceName = "Test Service Name",
            ServiceDescription = "Test Service Description",
            OccuredAt = DateTime.UtcNow
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.Remove(It.IsAny<string>()), Times.AtLeast(2));
    }
}