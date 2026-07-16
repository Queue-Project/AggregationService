using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetEmployeeRecommendations;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QUserService.Contracts.Interfaces;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.UseCases.QueryTests.
    GetEmployeeRecommendationQueryTests;

public class GetEmployeeRecommendationQueryTest
{
    private readonly Mock<ILogger<GetEmployeeRecommendationQueryHandler>> _mockLogger;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly GetEmployeeRecommendationQueryHandler _handler;

    public GetEmployeeRecommendationQueryTest()
    {
        _mockLogger = new Mock<ILogger<GetEmployeeRecommendationQueryHandler>>();
        _mockUserService = new Mock<IUserService>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _handler = new GetEmployeeRecommendationQueryHandler(_mockLogger.Object,
              _mockRecommendationService.Object, _mockUserService.Object);
    }


    [Fact]
    public async Task Handler_Should_Return_Recommended_Result_When_Request_Is_Valid()
    {
        //Arrange
        var recommendationRequest = new GetRecommendedEmployeesRequest
        {
            CompanyId = 1,
            BranchId = 1,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15
        };

        var expectedRecommendationResponse = TestData.EmployeeExpectedResponse();
        _mockRecommendationService.Setup(s => s.GetRecommendedEmployees(It.IsAny<GetRecommendedEmployeesRequest>()))
            .Returns(UnaryResult.FromResult(expectedRecommendationResponse));

        List<int> ids = [1];
        var expectedUserResponse = TestData.EmployeeDetailsResponses();
        _mockUserService.Setup(s => s.GetEmployeeDetails(ids))
            .Returns(UnaryResult.FromResult(expectedUserResponse));


        var query = new GetEmployeeRecommendationQuery(recommendationRequest.CompanyId,
            recommendationRequest.BranchId,recommendationRequest.ServiceId,recommendationRequest.PageNumber);

        //Act

        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.PageNumber.ShouldBe(recommendationRequest.PageNumber);
        result.PageSize.ShouldBe(recommendationRequest.PageSize);
        result.TotalCount.ShouldBe(1);
        result.HasNextPage.ShouldBe(false);
        result.HasPreviousPage.ShouldBe(false);
        result.TotalPages.ShouldBe(1);

        var firstItem = result.Items.FirstOrDefault();
        firstItem!.CompanyId.ShouldBe(1);
        firstItem.BranchId.ShouldBe(1);
        firstItem.ServiceId.ShouldBe(1);
        firstItem.EmployeeId.ShouldBe(1);
    }
}