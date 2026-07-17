using BranchService.Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Responses.QueueResponses;
using QContracts.Interfaces;
using QUserService.Contracts.Interfaces;

namespace QAggregationService.Application.UseCases.Queries.QueueTrackingQueries.GetCustomerActiveQueues;

public class
    GetCustomerActiveQueuesQueryHandler : IRequestHandler<GetCustomerActiveQueuesQuery,
    List<CustomerActiveQueueResponse>>
{
    private readonly ILogger<GetCustomerActiveQueuesQueryHandler> _logger;
    private readonly IUserService _userService;
    private readonly IBranchService _branchService;
    private readonly IQueueService _queueService;

    public GetCustomerActiveQueuesQueryHandler(ILogger<GetCustomerActiveQueuesQueryHandler> logger,
        IUserService userService, IBranchService branchService, IQueueService queueService)
    {
        _logger = logger;
        _userService = userService;
        _branchService = branchService;
        _queueService = queueService;
    }

    public async Task<List<CustomerActiveQueueResponse>> Handle(GetCustomerActiveQueuesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching active queues");

        var activeQueues = await _queueService.GetCustomerActiveQueues(request.CustomerId);
        if (!activeQueues.Any())
        {
            _logger.LogWarning("Not found any active queues!");
            return new List<CustomerActiveQueueResponse>();
        }

        var serviceIds = activeQueues.Select(s => s.ServiceId).ToList();
        var employeeIds = activeQueues.Select(s => s.EmployeeId).ToList();

        var services = await _branchService.GetServiceDetails(serviceIds);
        var employees = await _userService.GetEmployeeDetails(employeeIds);

        var serviceDictionary = services.ToDictionary(s => s.ServiceId);
        var employeeDictionary = employees.ToDictionary(s => s.EmployeeId);
        

        var response = activeQueues.Where(s =>
                serviceDictionary.ContainsKey(s.ServiceId) &&
                employeeDictionary.ContainsKey(s.EmployeeId))
            .Select(s =>
            {
                var service = serviceDictionary[s.ServiceId];
                var employee = employeeDictionary[s.EmployeeId];

                return new CustomerActiveQueueResponse()
                {
                    QueueId = s.QueueId,
                    CompanyId = service.CompanyId,
                    CompanyName = service.CompanyName,
                    BranchId = service.BranchId,
                    BranchName = service.BranchName,
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    EmployeeId = employee.EmployeeId,
                    EmployeeFullName = $"{employee.FirstName} {employee.LastName}",
                    StartTime = s.StartTime,
                    Status = s.Status
                };
            })
            .OrderBy(s => s.StartTime)
            .ToList();


        return response;
    }
}