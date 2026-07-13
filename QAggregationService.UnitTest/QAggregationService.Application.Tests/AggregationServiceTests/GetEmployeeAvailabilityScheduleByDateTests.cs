using System.Net;
using BranchService.Contracts.Interfaces;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Exceptions;
using QAggregationService.Application.Services;
using QAggregationService.Contracts.Requests;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QContracts.Interfaces;
using QContracts.Requests;
using QUserService.Contracts.Interfaces;
using QUserService.Contracts.Requests.EmployeeRequests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.AggregationServiceTests;

public class GetEmployeeAvailabilityScheduleByDateTests
{
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<AggregationService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;
    private readonly AggregationService _aggregationService;

    public GetEmployeeAvailabilityScheduleByDateTests()
    {
        _mockQueueService = new Mock<IQueueService>();
        _mockBranchService = new Mock<IBranchService>();
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<AggregationService>>();
        _mockCacheService = new Mock<ICacheService>();
        _mockMemoryCacheService = new Mock<IMemoryCacheService>();
        _aggregationService = new AggregationService(_mockQueueService.Object, _mockBranchService.Object,
            _mockLogger.Object, _mockCacheService.Object, _mockMemoryCacheService.Object, _mockUserService.Object);
    }
    
    [Fact]
    public async Task Service_Should_Return_Employee_Schedule_Successfully()
    {
        //Arrange
        var request = new GetEmployeeAvailabilityScheduleRequest()
        {
            EmployeeId = 1,
            Date = new DateOnly(2026,07,13)
        };

        var expectedEmployeeScheduleResponse = TestData.ScheduleInfo();
        _mockUserService.Setup(s => s.GetEmployeeSchedule(It.IsAny<EmployeeScheduleRequest>()))
            .Returns(UnaryResult.FromResult(expectedEmployeeScheduleResponse));

        var expectedEmployeeQueuesListResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetEmployeeQueuesByDate(It.IsAny<EmployeeQueuesByDateRequest>()))
            .Returns(UnaryResult.FromResult(expectedEmployeeQueuesListResponse));



        //Act
        var result = await _aggregationService.GetEmployeeAvailabilityScheduleByDate(request);

        
        //Assert
        
        result.EmployeeId.ShouldBe(request.EmployeeId);
        result.Date.ShouldBe(result.Date);
        var schedules = result.Schedules.First();
        schedules.ScheduleId.ShouldBe(1);
        schedules.Description.ShouldBe("Before lunch time working hours");
    }

    
    [Fact]
    public async Task Service_Should_Throw_When_Employee_Does_Not_Have_Schedule_For_Selected_day()
    {
        //Arrange
        var request = new GetEmployeeAvailabilityScheduleRequest()
        {
            EmployeeId = 1,
            Date = new DateOnly(2026,07,14)
        };

        var expectedEmployeeScheduleResponse = TestData.ScheduleInfoWithEmptySlots();
      
        _mockUserService.Setup(s => s.GetEmployeeSchedule(It.IsAny<EmployeeScheduleRequest>()))
            .Returns(UnaryResult.FromResult(expectedEmployeeScheduleResponse));

        var expectedEmployeeQueuesListResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetEmployeeQueuesByDate(It.IsAny<EmployeeQueuesByDateRequest>()))
            .Returns(UnaryResult.FromResult(expectedEmployeeQueuesListResponse));



        //Act
        var result =  _aggregationService.GetEmployeeAvailabilityScheduleByDate(request);

        
        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        exception.Message.ShouldBe($"Employee with Id {request.EmployeeId} does not have schedule for selected date {request.Date}");
    }
}