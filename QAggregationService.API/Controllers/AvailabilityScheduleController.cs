using Microsoft.AspNetCore.Mvc;
using QAggregationService.Contracts.Interfaces;
using QAggregationService.Contracts.Requests;
using QAggregationService.Contracts.Responses;

namespace QAggregationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityScheduleController : ControllerBase
{
    private readonly IAggregationService _aggregationService;
    private readonly ILogger<DashboardController> _logger;

    public AvailabilityScheduleController(IAggregationService aggregationService, ILogger<DashboardController> logger)
    {
        _aggregationService = aggregationService;
        _logger = logger;
    }

    [HttpGet("get-availability-schedule")]
    public async Task<ActionResult<CustomerReportResponse>> GetAvailabilitySchedule(
        [FromQuery] GetEmployeeAvailabilityScheduleRequest request)
    {
        _logger.LogInformation("Getting employee schedule - EmployeeId: {EmployeeId} for Date {Date}",
            request.EmployeeId, request.Date);

        var result = await _aggregationService.GetEmployeeAvailabilityScheduleByDate(request);

        return Ok(result);
    }
}