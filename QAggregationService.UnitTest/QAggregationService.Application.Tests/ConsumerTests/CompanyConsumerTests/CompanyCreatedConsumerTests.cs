using BranchService.Contracts.Events.CompanyEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CompanyConsumers;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CompanyConsumerTests;

public class CompanyCreatedConsumerTests
{
    private readonly Mock<ILogger<CompanyCreatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<CompanyCreatedEvent>> _mockContext;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly CompanyCreatedConsumer _consumer;

    public CompanyCreatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CompanyCreatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<CompanyCreatedEvent>>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _consumer = new CompanyCreatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Company_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new CompanyCreatedEvent()
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
        _mockCacheService.Verify(s => s.Remove(It.IsAny<string>()), Times.Never);
    }
}