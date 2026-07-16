using BranchService.Contracts.Interfaces;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetBranchRecommendations;

using QAggregationService.Application.UseCases.Queries.RecommendationService.GetServiceRecommendations;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.UseCases.QueryTests.
    GetServiceRecommendationQueryTests;

public class GetServiceRecommendationQueryTest
{
    private readonly Mock<ILogger<GetServiceRecommendationQueryHandler>> _mockLogger;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly GetServiceRecommendationQueryHandler _handler;

    public GetServiceRecommendationQueryTest()
    {
        _mockLogger = new Mock<ILogger<GetServiceRecommendationQueryHandler>>();
        _mockBranchService = new Mock<IBranchService>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _handler = new GetServiceRecommendationQueryHandler(_mockLogger.Object,
             _mockBranchService.Object, _mockRecommendationService.Object);
    }


    [Fact]
    public async Task Handler_Should_Return_Recommended_Result_When_Request_Is_Valid()
    {
        //Arrange
        var recommendationRequest = new GetRecommendedServicesRequest
        {
            CompanyId = 1,
            BranchId = 1,
            PageNumber = 1,
            PageSize = 15
        };

        var expectedRecommendationResponse = TestData.ServiceExpectedResponse();
        _mockRecommendationService.Setup(s => s.GetRecommendedServices(It.IsAny<GetRecommendedServicesRequest>()))
            .Returns(UnaryResult.FromResult(expectedRecommendationResponse));

        List<int> ids = [1];
        var expectedBranchResponse = TestData.ServiceDetailsResponses();
        _mockBranchService.Setup(s => s.GetServiceDetails(ids))
            .Returns(UnaryResult.FromResult(expectedBranchResponse));


        var query = new GetServiceRecommendationQuery(recommendationRequest.CompanyId,
            recommendationRequest.BranchId,recommendationRequest.PageNumber);

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
    }
}