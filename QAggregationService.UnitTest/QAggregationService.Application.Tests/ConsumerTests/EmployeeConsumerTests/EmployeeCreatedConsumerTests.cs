using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.EmployeeConsumers;
using QUserService.Contracts.Events.EmployeeEvent;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.EmployeeConsumerTests;

public class EmployeeCreatedConsumerTests
{
    private readonly Mock<ILogger<EmployeeCreatedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<EmployeeCreatedEvent>> _mockContext;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly EmployeeCreatedConsumer _consumer;

    public EmployeeCreatedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<EmployeeCreatedConsumer>>();
        _mockContext = new Mock<ConsumeContext<EmployeeCreatedEvent>>();
        _mockCacheService = new Mock<ICacheService>();
        _consumer = new EmployeeCreatedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Employee_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new EmployeeCreatedEvent
        {
            EmployeeId = 1,
            CompanyId = 1,
            BranchId = 1,
            ServiceId = 1,
            FirstName = "Test Firstname",
            LastName = "Test Lastname",
            PhoneNumber = "+992923324252",
            Position = "Test Position",
        };

        _mockContext.Setup(s => s.Message).Returns(expectedEvent);


        //Act
        await _consumer.Consume(_mockContext.Object);

        //Assert
        _mockCacheService.Verify(s => s.RemoveAsync(It.IsAny<string>()), Times.AtLeast(3));
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