using BranchService.Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Responses;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetCompanyRecommendations;

public class
    GetCompanyRecommendationQueryHandler : IRequestHandler<GetCompanyRecommendationQuery,
    PagedResponse<RecommendedCompanyResponse>>
{
    private const int PageSize = 15;
    private readonly ILogger<GetCompanyRecommendationQueryHandler> _logger;
    private readonly IBranchService _branchService;
    private readonly IRecommendationService _recommendationService;

    public GetCompanyRecommendationQueryHandler(ILogger<GetCompanyRecommendationQueryHandler> logger,
        IBranchService branchService, IRecommendationService recommendationService)
    {
        _logger = logger;
        _branchService = branchService;
        _recommendationService = recommendationService;
    }

    public async Task<PagedResponse<RecommendedCompanyResponse>> Handle(GetCompanyRecommendationQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching CompanyIds from RecommendationService");
        var recommendedCompanies = await _recommendationService.GetRecommendedCompanies(
            new GetRecommendedCompaniesRequest
            {
                CategoryId = request.CategoryId,
                PageNumber = request.PageNumber,
                PageSize = PageSize
            });

        var ids = recommendedCompanies.Items.Select(s => s.CompanyId).ToList();
        var companyDetails = await _branchService.GetCompanyDetails(ids);

        var companyDictionary = companyDetails
            .ToDictionary(x => x.CompanyId);
        var items = recommendedCompanies.Items
            .Where(x => companyDictionary.ContainsKey(x.CompanyId))
            .Select(x =>
            {
                var company = companyDictionary[x.CompanyId];

                return new RecommendedCompanyResponse
                {
                    CompanyId = x.CompanyId,
                    CompanyName = company.CompanyName,
                    CompanyCategory = company.CompanyCategory,
                    Address = company.Address,
                    PhoneNumber = company.PhoneNumber,
                    EmailAddress = company.EmailAddress,

                    RecommendationScore = x.RecommendationScore,
                    AverageRating = x.AverageRating,
                    ReviewCount = x.ReviewCount,
                    CompletedQueues = x.CompletedQueues
                };
            })
            .ToList();


        return new PagedResponse<RecommendedCompanyResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = PageSize,
            TotalCount = recommendedCompanies.TotalCount
        };
    }
}