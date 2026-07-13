using MessagePack;

namespace QAggregationService.Contracts.Requests;

[MessagePackObject]
public class GetEmployeeAvailabilityScheduleRequest
{
    [Key(0)] public int EmployeeId { get; set; }
    [Key(1)] public DateOnly Date { get; set; }
}