using MediatR;
using Microsoft.AspNetCore.Mvc;
using QAggregationService.Application.Responses.QueueResponses;
using QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetCustomerActiveQueues;
using QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetQueueTracking;

namespace QAggregationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueueTrackingController : ControllerBase
{
    private readonly IMediator _mediator;

    public QueueTrackingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-customer-active-queues/{customerId}")]
    public async Task<ActionResult<List<CustomerActiveQueueResponse>>> GetCustomerActiveQueues(
        [FromRoute] int customerId)
    {
        var query = new GetCustomerActiveQueuesQuery(customerId);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("get-queue-position/{queueId}")]
    public async Task<ActionResult<QueueTrackingResponse>> GetQueuePosition(
        [FromRoute] int queueId)
    {
        var query = new GetQueueTrackingQuery(queueId);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}