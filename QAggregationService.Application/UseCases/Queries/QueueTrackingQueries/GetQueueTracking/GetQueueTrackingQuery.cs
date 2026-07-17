using MediatR;
using QAggregationService.Application.Responses.QueueResponses;

namespace QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetQueueTracking;

public record GetQueueTrackingQuery(int QueueId):IRequest<QueueTrackingResponse>;