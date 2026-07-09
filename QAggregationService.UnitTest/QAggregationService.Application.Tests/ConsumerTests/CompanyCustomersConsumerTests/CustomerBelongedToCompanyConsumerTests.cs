using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.CompanyCustomersConsumer;
using QContracts.Events;
using QContracts.Events.Enums;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.CompanyCustomersConsumerTests;

public class CustomerBelongedToCompanyConsumerTests
{
    private readonly Mock<ILogger<CustomerBelongedToCompanyConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<QueueEvent>> _mockContext;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly CustomerBelongedToCompanyConsumer _consumer;

    public CustomerBelongedToCompanyConsumerTests()
    {
        _mockLogger = new Mock<ILogger<CustomerBelongedToCompanyConsumer>>();
        _mockContext = new Mock<ConsumeContext<QueueEvent>>();
        _mockCacheService = new Mock<ICacheService>();
        _consumer = new CustomerBelongedToCompanyConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Customers_From_Cache_When_Event_Received_With_Updated_Type()
    {
        //Arrange
        var expectedEvent = new QueueEvent()
        {
            CompanyId = 1,
            CustomerId = 1,
            EmployeeId = 1,
            QueueId = 1,
            Email = "test@gmail.com",
            StartTime = DateTimeOffset.UtcNow.Date.AddHours(1),
            EndTime = DateTimeOffset.UtcNow.Date.AddHours(2),
            Status = UpdatedQueueStatus.Confirmed,
            CancelReason = null,
            EventType = QueueEventType.Updated
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.RemoveAsync(It.IsAny<string>()), Times.AtLeast(2));
    }
    
    [Fact]
    public async Task Consume_Should_Remove_Customers_From_Cache_When_Event_Received_With_Created_Type()
    {
        //Arrange
        var expectedEvent = new QueueEvent()
        {
            CompanyId = 1,
            CustomerId = 1,
            EmployeeId = 1,
            QueueId = 1,
            Email = "test@gmail.com",
            StartTime = DateTimeOffset.UtcNow.Date.AddHours(1),
            EndTime = DateTimeOffset.UtcNow.Date.AddHours(2),
            Status = UpdatedQueueStatus.Confirmed,
            CancelReason = null,
            EventType = QueueEventType.Created
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