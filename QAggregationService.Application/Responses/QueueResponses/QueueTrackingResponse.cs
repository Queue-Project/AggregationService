using QContracts.Events.Enums;

namespace QAggregationService.Application.Responses.QueueResponses;

public class QueueTrackingResponse
{
    public int QueueId { get; set; }

    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;

    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; } = string.Empty;

    public DateTimeOffset StartTime { get; set; }

    public UpdatedQueueStatus Status { get; set; }

    public int Position { get; set; }

    public int CustomersAhead { get; set; }

    public int EstimatedWaitMinutes { get; set; }

}