namespace QAggregationService.Application.Responses;

public class RecommendedServiceResponse
{
    public int ServiceId { get; set; }
    public int CompanyId { get; set; }

    public int BranchId { get; set; }


    public string ServiceName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty;

    public int Duration { get; set; }

    public double RecommendationScore { get; set; }

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }

    public int CompletedQueues { get; set; }
}