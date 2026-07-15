using BranchService.Contracts.Interfaces;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetCompanyRecommendations;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.UseCases.QueryTests.
    GetCompanyRecommendationQueryTests;

public class GetCompanyRecommendationQueryTest
{
    private readonly Mock<ILogger<GetCompanyRecommendationQueryHandler>> _mockLogger;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly GetCompanyRecommendationQueryHandler _handler;

    public GetCompanyRecommendationQueryTest()
    {
        _mockLogger = new Mock<ILogger<GetCompanyRecommendationQueryHandler>>();
        _mockBranchService = new Mock<IBranchService>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _handler = new GetCompanyRecommendationQueryHandler(_mockLogger.Object, _mockBranchService.Object,
            _mockRecommendationService.Object);
    }


    [Fact]
    public async Task Handler_Should_Return_Recommended_Result_When_Request_Is_Valid()
    {
        //Arrange
        var recommendationRequest = new GetRecommendedCompaniesRequest
        {
            CategoryId = 1,
            PageNumber = 1,
            PageSize = 15
        };

        var expectedRecommendationResponse = TestData.CompanyExpectedResponse();
        _mockRecommendationService.Setup(s => s.GetRecommendedCompanies(It.IsAny<GetRecommendedCompaniesRequest>()))
            .Returns(UnaryResult.FromResult(expectedRecommendationResponse));

        List<int> ids = [1];
        var expectedBranchResponse = TestData.CompanyDetailsResponses();
        _mockBranchService.Setup(s => s.GetCompanyDetails(ids))
            .Returns(UnaryResult.FromResult(expectedBranchResponse));


        var query = new GetCompanyRecommendationQuery(recommendationRequest.CategoryId,
            recommendationRequest.PageNumber);

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
    }
}