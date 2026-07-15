namespace QAggregationService.Application.Responses;

public class RecommendedEmployeeResponse
{
    public int EmployeeId { get; set; }

    public int ServiceId { get; set; }

    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;


    public double RecommendationScore { get; set; }

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }

    public int CompletedQueues { get; set; }
}