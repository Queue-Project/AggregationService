using BranchService.Contracts.Interfaces;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetCustomerActiveQueues;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QContracts.Interfaces;
using QContracts.Responses;
using QUserService.Contracts.Interfaces;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.UseCases.QueryTests.
    QueueTrackingQueriesTests;

public class GetCustomerActiveQueuesQueryHandlerTests
{
    private readonly Mock<ILogger<GetCustomerActiveQueuesQueryHandler>> _mockLogger;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly GetCustomerActiveQueuesQueryHandler _handler;

    public GetCustomerActiveQueuesQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetCustomerActiveQueuesQueryHandler>>();
        _mockUserService = new Mock<IUserService>();
        _mockBranchService = new Mock<IBranchService>();
        _mockQueueService = new Mock<IQueueService>();
        _handler = new GetCustomerActiveQueuesQueryHandler(_mockLogger.Object, _mockUserService.Object,
            _mockBranchService.Object, _mockQueueService.Object);
    }

    [Fact]
    public async Task Handler_Should_Return_Customer_Active_Queues_When_Request_Is_Valid()
    {
        //Arrange
        var query = new GetCustomerActiveQueuesQuery(1);

        var activeQueuesExpectedResponse = TestData.ActiveQueues();
        _mockQueueService.Setup(s => s.GetCustomerActiveQueues(query.CustomerId))
            .Returns(UnaryResult.FromResult(activeQueuesExpectedResponse));

        var serviceIds = activeQueuesExpectedResponse.Select(s => s.ServiceId).ToList();
        var employeeIds = activeQueuesExpectedResponse.Select(s => s.EmployeeId).ToList();

        var serviceDetailsExpectedResponse = TestData.ServiceDetailsResponses2();
        _mockBranchService.Setup(s => s.GetServiceDetails(serviceIds))
            .Returns(UnaryResult.FromResult(serviceDetailsExpectedResponse));
        var employeeDetailsExpectedResponse = TestData.EmployeeDetailsResponses2();
        _mockUserService.Setup(s => s.GetEmployeeDetails(employeeIds))
            .Returns(UnaryResult.FromResult(employeeDetailsExpectedResponse));

        //Act

        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldNotBeEmpty();
        var firstQueue = result.FirstOrDefault();
        var firstEmployee = employeeDetailsExpectedResponse.FirstOrDefault();
        var firstService = serviceDetailsExpectedResponse.FirstOrDefault();
        firstQueue!.QueueId.ShouldBe(1);
        firstQueue.EmployeeId.ShouldBe(1);
        firstQueue.ServiceId.ShouldBe(1);
        firstQueue.BranchId.ShouldBe(1);
        firstQueue.CompanyId.ShouldBe(1);
        firstQueue.EmployeeFullName.ShouldBe($"{firstEmployee!.FirstName} {firstEmployee.LastName}");
        firstQueue.CompanyName.ShouldBe(firstService!.CompanyName);
        firstQueue.BranchName.ShouldBe(firstService.BranchName);
        firstQueue.ServiceName.ShouldBe(firstService.ServiceName);
    }

    [Fact]
    public async Task Handler_Should_Return_Empty_List_When_Active_Queues_Empty()
    {
        //Arrange
        var query = new GetCustomerActiveQueuesQuery(1);

        var activeQueuesExpectedResponse = new List<CustomerQueueResponse>();

        _mockQueueService.Setup(s => s.GetCustomerActiveQueues(query.CustomerId))
            .Returns(UnaryResult.FromResult(activeQueuesExpectedResponse));


        //Act

        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert

        result.ShouldBeEmpty();
    }
}