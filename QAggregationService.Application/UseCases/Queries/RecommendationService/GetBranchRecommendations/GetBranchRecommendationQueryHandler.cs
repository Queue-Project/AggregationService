using BranchService.Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Responses;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetBranchRecommendations;

public class
    GetBranchRecommendationQueryHandler : IRequestHandler<GetBranchRecommendationQuery,
    PagedResponse<RecommendedBranchResponse>>
{
    private const int PageSize = 15;
    private readonly ILogger<GetBranchRecommendationQueryHandler> _logger;
    private readonly IBranchService _branchService;
    private readonly IRecommendationService _recommendationService;

    public GetBranchRecommendationQueryHandler(ILogger<GetBranchRecommendationQueryHandler> logger,
        IBranchService branchService, IRecommendationService recommendationService)
    {
        _logger = logger;
        _branchService = branchService;
        _recommendationService = recommendationService;
    }

    public async Task<PagedResponse<RecommendedBranchResponse>> Handle(GetBranchRecommendationQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching BranchIds from RecommendationService");
        var recommendedBranches = await _recommendationService.GetRecommendedBranches(
            new GetRecommendedBranchesRequest()
            {
                CompanyId = request.CompanyId,
                PageNumber = request.PageNumber,
                PageSize = PageSize
            });

        var ids = recommendedBranches.Items.Select(s => s.BranchId).ToList();
        var branchDetails = await _branchService.GetBranchDetails(ids);

        var branchDictionary = branchDetails
            .ToDictionary(x => x.BranchId);
        var items = recommendedBranches.Items
            .Where(x => branchDictionary.ContainsKey(x.BranchId))
            .Select(x =>
            {
                var branch = branchDictionary[x.BranchId];

                return new RecommendedBranchResponse()
                {
                    BranchId = x.BranchId,
                    CompanyId = branch.CompanyId,
                    BranchName = branch.BranchName,
                    CompanyName = branch.CompanyName,
                    Address = branch.Address,
                    RecommendationScore = x.RecommendationScore,
                    AverageRating = x.AverageRating,
                    ReviewCount = x.ReviewCount,
                    CompletedQueues = x.CompletedQueues
                };
            })
            .ToList();


        return new PagedResponse<RecommendedBranchResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = PageSize,
            TotalCount = recommendedBranches.TotalCount
        };
    }
}