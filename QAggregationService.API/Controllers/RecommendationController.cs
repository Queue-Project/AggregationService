using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QAggregationService.Application.Responses;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetBranchRecommendations;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetCompanyRecommendations;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetEmployeeRecommendations;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetServiceRecommendations;

namespace QAggregationService.API.Controllers;

[ApiController]
[Route("api[controller]")]
[Authorize]
public class RecommendationController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-recommended-companies")]
    public async Task<ActionResult<PagedResponse<RecommendedCompanyResponse>>> GetRecommendedCompanies(
        [FromQuery] GetCompanyRecommendationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-recommended-branches")]
    public async Task<ActionResult<PagedResponse<RecommendedCompanyResponse>>> GetRecommendedBranches(
        [FromQuery] GetBranchRecommendationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-recommended-services")]
    public async Task<ActionResult<PagedResponse<RecommendedCompanyResponse>>> GetRecommendedServices(
        [FromQuery] GetServiceRecommendationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-recommended-employees")]
    public async Task<ActionResult<PagedResponse<RecommendedCompanyResponse>>> GetRecommendedEmployees(
        [FromQuery] GetEmployeeRecommendationQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}