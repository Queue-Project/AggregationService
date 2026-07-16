namespace QAggregationService.Application.Responses;

public class RecommendedBranchResponse
{
    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public string BranchName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    

    public string Address { get; set; } = string.Empty;

   

    public double RecommendationScore { get; set; }

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }

    public int CompletedQueues { get; set; }
}