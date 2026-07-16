using BranchService.Contracts.Events.Enums;

namespace QAggregationService.Application.Responses;

public class RecommendedCompanyResponse
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    
    public CompanyCategory CompanyCategory { get; set; }

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public double RecommendationScore { get; set; }

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }

    public int CompletedQueues { get; set; }
}