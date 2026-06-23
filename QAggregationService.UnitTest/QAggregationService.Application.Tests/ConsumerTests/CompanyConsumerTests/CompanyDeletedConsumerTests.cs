using BranchService.Contracts.Events.CompanyEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CompanyConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CompanyConsumerTests;

public class CompanyDeletedConsumerTests
{
    private readonly Mock<ILogger<CompanyDeletedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<CompanyDeletedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly CompanyDeletedConsumer _consumer;

    public CompanyDeletedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CompanyDeletedConsumer>>();
        _mockContext = new Mock<ConsumeContext<CompanyDeletedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new CompanyDeletedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Company_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new CompanyDeletedEvent()
        {
            CompanyId = 1,
            CompanyName = "Test Company Name",
            EmailAddress = "test@gmail.com",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            OccuredAt = DateTime.UtcNow
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.Remove(It.IsAny<string>()), Times.Once);
    }
}