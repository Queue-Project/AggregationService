using MediatR;
using QAggregationService.Application.Responses;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetEmployeeRecommendations;

public record GetEmployeeRecommendationQuery(int CompanyId, int BranchId, int ServiceId, int PageNumber): IRequest<PagedResponse<RecommendedEmployeeResponse>>;