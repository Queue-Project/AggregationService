using BranchService.Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Responses;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetServiceRecommendations;

public class
    GetEmployeeRecommendationQueryHandler : IRequestHandler<GetServiceRecommendationQuery,
    PagedResponse<RecommendedServiceResponse>>
{
    private const int PageSize = 15;
    private readonly ILogger<GetEmployeeRecommendationQueryHandler> _logger;
    private readonly IBranchService _branchService;
    private readonly IRecommendationService _recommendationService;

    public GetEmployeeRecommendationQueryHandler(ILogger<GetEmployeeRecommendationQueryHandler> logger,
        IBranchService branchService, IRecommendationService recommendationService)
    {
        _logger = logger;
        _branchService = branchService;
        _recommendationService = recommendationService;
    }

    public async Task<PagedResponse<RecommendedServiceResponse>> Handle(GetServiceRecommendationQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching ServiceIds from RecommendationService");
        var recommendedServices = await _recommendationService.GetRecommendedServices(
            new GetRecommendedServicesRequest()
            {
                CompanyId = request.CompanyId,
                BranchId = request.BranchId,
                PageNumber = request.PageNumber,
                PageSize = PageSize
            });

        var ids = recommendedServices.Items.Select(s => s.ServiceId).ToList();
        var serviceDetails = await _branchService.GetServiceDetails(ids);

        var serviceDictionary = serviceDetails
            .ToDictionary(x => x.ServiceId);
        var items = recommendedServices.Items
            .Where(x => serviceDictionary.ContainsKey(x.ServiceId))
            .Select(x =>
            {
                var service = serviceDictionary[x.ServiceId];

                return new RecommendedServiceResponse()
                {
                    ServiceId = x.ServiceId,
                    BranchId = service.BranchId,
                    CompanyId = service.CompanyId,
                    ServiceName = service.ServiceName,
                    BranchName = service.BranchName,
                    CompanyName = service.CompanyName,
                    Description = service.Description ?? "",
                    Duration = service.ServiceDuration,
                    RecommendationScore = x.RecommendationScore,
                    AverageRating = x.AverageRating,
                    ReviewCount = x.ReviewCount,
                    CompletedQueues = x.CompletedQueues
                };
            })
            .ToList();


        return new PagedResponse<RecommendedServiceResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = PageSize,
            TotalCount = recommendedServices.TotalCount
        };
    }
}