using MediatR;
using Microsoft.Extensions.Logging;
using QAggregationService.Application.Responses;
using QUserService.Contracts.Interfaces;
using RecommendationService.Contracts.Interfaces;
using RecommendationService.Contracts.Requests;

namespace QAggregationService.Application.UseCases.Queries.RecommendationService.GetEmployeeRecommendations;

public class
    GetEmployeeRecommendationQueryHandler : IRequestHandler<GetEmployeeRecommendationQuery,
    PagedResponse<RecommendedEmployeeResponse>>
{
    private const int PageSize = 15;
    private readonly ILogger<GetEmployeeRecommendationQueryHandler> _logger;
    private readonly IUserService _userService;
    private readonly IRecommendationService _recommendationService;

    public GetEmployeeRecommendationQueryHandler(ILogger<GetEmployeeRecommendationQueryHandler> logger,
         IRecommendationService recommendationService, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        _recommendationService = recommendationService;
        _userService = userService;
    }

    public async Task<PagedResponse<RecommendedEmployeeResponse>> Handle(GetEmployeeRecommendationQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching EmployeeIds from RecommendationService");
        var recommendedEmployees = await _recommendationService.GetRecommendedEmployees(
            new GetRecommendedEmployeesRequest()
            {
                CompanyId = request.CompanyId,
                BranchId = request.BranchId,
                ServiceId = request.ServiceId,
                PageNumber = request.PageNumber,
                PageSize = PageSize
            });

        var ids = recommendedEmployees.Items.Select(s => s.EmployeeId).ToList();
        var employeeDetails = await _userService.GetEmployeeDetails(ids);

        var employeeDictionary = employeeDetails
            .ToDictionary(x => x.EmployeeId);
        var items = recommendedEmployees.Items
            .Where(x => employeeDictionary.ContainsKey(x.EmployeeId))
            .Select(x =>
            {
                var employee = employeeDictionary[x.EmployeeId];

                return new RecommendedEmployeeResponse()
                {
                    EmployeeId = x.EmployeeId,
                    ServiceId = employee.CompanyServiceId ??0,
                    BranchId = employee.BranchId ?? 0,
                    CompanyId = employee.CompanyId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position = employee.Position,
                    PhoneNumber = employee.PhoneNumber,
                    EmailAddress =employee.EmailAddress,
                    RecommendationScore = x.RecommendationScore,
                    AverageRating = x.AverageRating,
                    ReviewCount = x.ReviewCount,
                    CompletedQueues = x.CompletedQueues
                };
            })
            .ToList();


        return new PagedResponse<RecommendedEmployeeResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = PageSize,
            TotalCount = recommendedEmployees.TotalCount
        };
    }
}