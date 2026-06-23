using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Consumers.EmployeeConsumers;
using QAuthService.Contracts.Events.EmployeeEvent;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.ConsumerTests.EmployeeConsumerTests;

public class EmployeeDeletedConsumerTests
{
    private readonly Mock<ILogger<EmployeeDeletedConsumer>> _mockLogger;
    private readonly Mock<ConsumeContext<EmployeeDeletedEvent>> _mockContext;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly EmployeeDeletedConsumer _consumer;

    public EmployeeDeletedConsumerTests()
    {
        _mockLogger = new Mock<ILogger<EmployeeDeletedConsumer>>();
        _mockContext = new Mock<ConsumeContext<EmployeeDeletedEvent>>();
        _mockCacheService = new Mock<ICacheService>();
        _consumer = new EmployeeDeletedConsumer(_mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Consume_Should_Remove_Employee_From_Cache_When_Event_Received()
    {
        //Arrange
        var expectedEvent = new EmployeeDeletedEvent
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