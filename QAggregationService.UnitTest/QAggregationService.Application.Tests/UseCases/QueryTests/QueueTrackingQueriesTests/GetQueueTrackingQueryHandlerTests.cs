using System.Net;
using BranchService.Contracts.Interfaces;
using BranchService.Contracts.Responses;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Exceptions;
using QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetQueueTracking;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QContracts.Interfaces;
using QUserService.Contracts.Interfaces;
using QUserService.Contracts.Requests.EmployeeRequests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.UseCases.QueryTests.
    QueueTrackingQueriesTests;

public class GetQueueTrackingQueryHandlerTests
{
    private readonly Mock<ILogger<GetQueueTrackingQueryHandler>> _mockLogger;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly GetQueueTrackingQueryHandler _handler;

    public GetQueueTrackingQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetQueueTrackingQueryHandler>>();
        _mockUserService = new Mock<IUserService>();
        _mockBranchService = new Mock<IBranchService>();
        _mockQueueService = new Mock<IQueueService>();
        _handler = new GetQueueTrackingQueryHandler(_mockLogger.Object, _mockUserService.Object,
            _mockBranchService.Object, _mockQueueService.Object);
    }

    [Fact]
    public async Task Handler_Should_Return_QueuePosition_When_Request_Is_Valid()
    {
        //Arrange
        var query = new GetQueueTrackingQuery(1);

        var queueTrackingDataExpectedResponse = TestData.QueueTrackingData();
        _mockQueueService.Setup(s => s.GetQueueTrackingData(query.QueueId))
            .Returns(UnaryResult.FromResult(queueTrackingDataExpectedResponse));


        var queueServiceExpectedResponse = TestData.QueueTrackingDataService();
        _mockBranchService
            .Setup(s => s.GetServiceDetails(new List<int> { queueTrackingDataExpectedResponse.CompanyServiceId }))
            .Returns(UnaryResult.FromResult(queueServiceExpectedResponse));

        var employeeExpectedResponse = TestData.EmployeeResponse();
        _mockUserService.Setup(s => s.GetEmployeeById(It.IsAny<EmployeeByIdRequest>()))
            .Returns(UnaryResult.FromResult(employeeExpectedResponse));


        //Act

        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldNotBeNull();
        result.QueueId.ShouldBe(queueTrackingDataExpectedResponse.QueueId);
        result.EmployeeId.ShouldBe(queueTrackingDataExpectedResponse.EmployeeId);
        result.ServiceId.ShouldBe(queueTrackingDataExpectedResponse.CompanyServiceId);
        result.EmployeeFullName.ShouldBe($"{employeeExpectedResponse.FirstName} {employeeExpectedResponse.LastName}");
        result.Position.ShouldBe(2);
        result.CustomersAhead.ShouldBe(1);
    }

    [Fact]
    public async Task Handler_Should_Throw_When_Service_Not_Found()
    {
        //Arrange
        var query = new GetQueueTrackingQuery(1);

        var queueTrackingDataExpectedResponse = TestData.QueueTrackingData();
        _mockQueueService.Setup(s => s.GetQueueTrackingData(query.QueueId))
            .Returns(UnaryResult.FromResult(queueTrackingDataExpectedResponse));


        var queueServiceExpectedResponse = new List<ServiceDetailsResponse>();
        _mockBranchService
            .Setup(s => s.GetServiceDetails(new List<int> { queueTrackingDataExpectedResponse.CompanyServiceId }))
            .Returns(UnaryResult.FromResult(queueServiceExpectedResponse));


        //Act

        var result = _handler.Handle(query, CancellationToken.None);

        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Service with Id {queueTrackingDataExpectedResponse.CompanyServiceId} not found!");
    }

    [Fact]
    public async Task Handler_Should_Throw_When_Employee_Not_Found()
    {
        //Arrange
        var query = new GetQueueTrackingQuery(1);

        var queueTrackingDataExpectedResponse = TestData.QueueTrackingData();
        _mockQueueService.Setup(s => s.GetQueueTrackingData(query.QueueId))
            .Returns(UnaryResult.FromResult(queueTrackingDataExpectedResponse));


        var queueServiceExpectedResponse = TestData.QueueTrackingDataService();
        _mockBranchService
            .Setup(s => s.GetServiceDetails(new List<int> { queueTrackingDataExpectedResponse.CompanyServiceId }))
            .Returns(UnaryResult.FromResult(queueServiceExpectedResponse));

        var employeeExpectedResponse = TestData.EmployeeResponse();
        employeeExpectedResponse.IsValid = false;
        _mockUserService.Setup(s => s.GetEmployeeById(It.IsAny<EmployeeByIdRequest>()))
            .Returns(UnaryResult.FromResult(employeeExpectedResponse));


        //Act

        var result = _handler.Handle(query, CancellationToken.None);

        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Employee with Id {queueTrackingDataExpectedResponse.EmployeeId} not found");
    }
}