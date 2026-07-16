using MediatR;
using QAggregationService.Application.Responses;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetBranchRecommendations;

public record GetBranchRecommendationQuery(int CompanyId, int PageNumber): IRequest<PagedResponse<RecommendedBranchResponse>>;