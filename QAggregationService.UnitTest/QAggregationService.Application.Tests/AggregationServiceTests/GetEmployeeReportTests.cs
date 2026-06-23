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
using QUserService.Contracts.Interfaces;
using QUserService.Contracts.Responses.EmployeeResponses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.AggregationServiceTests;

public class GetEmployeeReportTests
{
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<AggregationService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;
    private readonly AggregationService _aggregationService;

    public GetEmployeeReportTests()
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
    public async Task Service_Should_Return_Employee_Report_Successfully()
    {
        //Arrange
        var request = new EmployeeReportRequest()
        {
            EmployeeId = 1,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };

        var expectedEmployeeListResponse = TestData.EmployeeInfos();
        _mockUserService.Setup(s => s.GetAllEmployees())
            .Returns(UnaryResult.FromResult(expectedEmployeeListResponse));
            
        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<List<EmployeeInfo>>>>(),
                It.IsAny<TimeSpan>(), 
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<EmployeeInfo>>> factory, TimeSpan absoluteExpiration, TimeSpan slidingExpiration) =>
            {
                
                return factory().Result;
            });

        var expectedEmployeeQueuesListResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetEmployeeQueuesAsync(request.EmployeeId))
            .Returns(UnaryResult.FromResult(expectedEmployeeQueuesListResponse));

        var expectedEmployeeReviewsListResponse = TestData.ReviewInfos();

        _mockQueueService.Setup(s => s.GetEmployeeReviewsAsync(request.EmployeeId))
            .Returns(UnaryResult.FromResult(expectedEmployeeReviewsListResponse));

        var expectedEmployeeComplaintsListResponse = TestData.ComplaintInfos();

        _mockQueueService.Setup(s => s.GetEmployeeComplaintsAsync(request.EmployeeId))
            .Returns(UnaryResult.FromResult(expectedEmployeeComplaintsListResponse));


        //Act
        var result = await _aggregationService.GetEmployeeReport(request);

        
        //Assert
        
        result.EmployeeId.ShouldBe(request.EmployeeId);
        result.DidNotComeQueues.ShouldBe(0);
        result.CompletedQueues.ShouldBe(2);
        result.TotalComplaintsReceived.ShouldBe(1);
        result.TotalReviewsReceived.ShouldBe(2);
        result.EmployeeName.ShouldBe("Test Firstname");
        result.CancelledQueues.ShouldBe(1);
        
    }

    [Fact]
    public async Task Service_Should_Throw_When_Employee_Not_Found()
    {
        //Arrange
        var request = new EmployeeReportRequest()
        {
            EmployeeId = 10,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };

        var expectedEmployeeListResponse = TestData.EmployeeInfos();
        _mockUserService.Setup(s => s.GetAllEmployees())
            .Returns(UnaryResult.FromResult(expectedEmployeeListResponse));
            
        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<List<EmployeeInfo>>>>(),
                It.IsAny<TimeSpan>(), 
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<EmployeeInfo>>> factory, TimeSpan absoluteExpiration, TimeSpan slidingExpiration) =>
            {
                
                return factory().Result;
            });

        //Act
        var result =  _aggregationService.GetEmployeeReport(request);

        
        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Not found employee with Id {request.EmployeeId}");
    }
    
    [Fact]
    public async Task Service_Should_Throw_When_Employee_List_Is_Empty()
    {
        //Arrange
        var request = new EmployeeReportRequest()
        {
            EmployeeId = 1,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };
        

        //Act
        var result =  _aggregationService.GetEmployeeReport(request);

        
        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Not found any employee");
    }
}