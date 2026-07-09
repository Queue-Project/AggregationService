using BranchService.Contracts.Events.CompanyServiceEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CompanyServiceConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CompanyServiceConsumerTests;

public class CompanyServiceDeletedConsumerTests
{
    private readonly Mock<ILogger<CompanyServiceDeletedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<CompanyServiceDeletedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly CompanyServiceDeletedConsumer _consumer;

    public CompanyServiceDeletedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CompanyServiceDeletedConsumer>>();
        _mockContext = new Mock<ConsumeContext<CompanyServiceDeletedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new CompanyServiceDeletedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Branch_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new CompanyServiceDeletedEvent()
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