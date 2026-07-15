using MediatR;
using QAggregationService.Application.Responses;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetCompanyRecommendations;

public record GetCompanyRecommendationQuery(int CategoryId, int PageNumber): IRequest<PagedResponse<RecommendedCompanyResponse>>;