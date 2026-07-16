using MediatR;
using QAggregationService.Application.Responses;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetServiceRecommendations;

public record GetServiceRecommendationQuery(int CompanyId, int BranchId, int PageNumber): IRequest<PagedResponse<RecommendedServiceResponse>>;