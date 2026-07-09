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
using QUserService.Contracts.Responses.CustomerResponses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.AggregationServiceTests;

public class GetCustomerReportTests
{
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<AggregationService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;
    private readonly AggregationService _aggregationService;

    public GetCustomerReportTests()
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
    public async Task Service_Should_Return_Customer_Report_Successfully()
    {
        //Arrange
        var request = new CustomerReportRequest()
        {
            CustomerId = 1,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };

        var expectedCustomerListResponse = TestData.CustomerInfos();
        _mockUserService.Setup(s => s.GetAllCustomers())
            .Returns(UnaryResult.FromResult(expectedCustomerListResponse));

        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<CustomerInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<CustomerInfo>>> factory, TimeSpan absoluteExpiration,
                TimeSpan slidingExpiration) =>
            {
                return factory().Result;
            });

        var expectedCustomerQueuesListResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetCustomerQueuesAsync(request.CustomerId))
            .Returns(UnaryResult.FromResult(expectedCustomerQueuesListResponse));

        var expectedCustomerReviewsListResponse = TestData.ReviewInfos();

        _mockQueueService.Setup(s => s.GetCustomerReviewsAsync(request.CustomerId))
            .Returns(UnaryResult.FromResult(expectedCustomerReviewsListResponse));

        var expectedCustomerComplaintsListResponse = TestData.ComplaintInfos();

        _mockQueueService.Setup(s => s.GetCustomerComplaintsAsync(request.CustomerId))
            .Returns(UnaryResult.FromResult(expectedCustomerComplaintsListResponse));


        //Act
        var result = await _aggregationService.GetCustomerReport(request);


        //Assert

        result.CustomerId.ShouldBe(request.CustomerId);
        result.DidNotComeQueues.ShouldBe(0);
        result.CompletedQueues.ShouldBe(2);
        result.TotalComplaints.ShouldBe(1);
        result.TotalReviews.ShouldBe(2);
        result.CustomerName.ShouldBe("Test Firstname");
        result.CancelledQueues.ShouldBe(1);
    }

    [Fact]
    public async Task Service_Should_Throw_When_Customer_Not_Found()
    {
        //Arrange
        var request = new CustomerReportRequest()
        {
            CustomerId = 10,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };

        var expectedCustomerListResponse = TestData.CustomerInfos();
        _mockUserService.Setup(s => s.GetAllCustomers())
            .Returns(UnaryResult.FromResult(expectedCustomerListResponse));

        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<CustomerInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<CustomerInfo>>> factory, TimeSpan absoluteExpiration,
                TimeSpan slidingExpiration) =>
            {
                return factory().Result;
            });

        //Act
        var result = _aggregationService.GetCustomerReport(request);


        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Not found customer with Id {request.CustomerId}");
    }

    [Fact]
    public async Task Service_Should_Throw_When_Customer_List_Is_Empty()
    {
        //Arrange
        var request = new CustomerReportRequest()
        {
            CustomerId = 1,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };


        //Act
        var result = _aggregationService.GetCustomerReport(request);


        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Not found any customer");
    }
}