using System.Net;
using BranchService.Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Exceptions;
using QAggregationService.Application.Responses.QueueResponses;
using QContracts.Interfaces;
using QUserService.Contracts.Interfaces;
using QUserService.Contracts.Requests.EmployeeRequests;

namespace QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetQueueTracking;

public class GetQueueTrackingQueryHandler : IRequestHandler<GetQueueTrackingQuery, QueueTrackingResponse>
{
    private readonly ILogger<GetQueueTrackingQueryHandler> _logger;
    private readonly IUserService _userService;
    private readonly IBranchService _branchService;
    private readonly IQueueService _queueService;

    public GetQueueTrackingQueryHandler(ILogger<GetQueueTrackingQueryHandler> logger,
        IUserService userService, IBranchService branchService, IQueueService queueService)
    {
        _logger = logger;
        _userService = userService;
        _branchService = branchService;
        _queueService = queueService;
    }

    public async Task<QueueTrackingResponse> Handle(GetQueueTrackingQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Fetching queue tracking data for QueueId: {QueueId}",
            request.QueueId);

        var queueTrackingData = await _queueService.GetQueueTrackingData(request.QueueId);
        var queueServices = await _branchService.GetServiceDetails([queueTrackingData.CompanyServiceId]);
        var queueService = queueServices.FirstOrDefault();
        if (queueService == null)
        {
            _logger.LogInformation("Service with Id {ServiceId} not found!", queueTrackingData.CompanyServiceId);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                $"Service with Id {queueTrackingData.CompanyServiceId} not found!");
        }

        var employee = await _userService.GetEmployeeById(new EmployeeByIdRequest
        {
            RequestId = Guid.NewGuid(),
            EmployeeId = queueTrackingData.EmployeeId
        });

        if (!employee.IsValid)
        {
            _logger.LogWarning("Employee with id {EmployeeId} not found", queueTrackingData.EmployeeId);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                $"Employee with Id {queueTrackingData.EmployeeId} not found");
        }

        var position = queueTrackingData.QueuesAhead.Count + 1;
        var queuesAhead = queueTrackingData.QueuesAhead.Count;

        var serviceIds = queueTrackingData.QueuesAhead.Select(s => s.CompanyServiceId).ToList();
        int estimatedWaitMinutes = 0;

        if (serviceIds.Count > 0)
        {
            var services = await _branchService.GetServiceDetails(serviceIds);
            estimatedWaitMinutes = services.Sum(s => s.ServiceDuration);
        }
        


        return new QueueTrackingResponse
        {
            QueueId = queueTrackingData.QueueId,
            CompanyId = queueService.CompanyId,
            CompanyName = queueService.CompanyName,
            BranchId = queueService.BranchId,
            BranchName = queueService.BranchName,
            ServiceId = queueService.ServiceId,
            ServiceName = queueService.ServiceName,
            EmployeeId = queueTrackingData.EmployeeId,
            EmployeeFullName = $"{employee.FirstName} {employee.LastName}",
            StartTime = queueTrackingData.StartTime,
            Status = queueTrackingData.Status,
            Position = position,
            CustomersAhead = queuesAhead,
            EstimatedWaitMinutes = estimatedWaitMinutes
        };
    }
}