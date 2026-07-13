using MessagePack;

namespace QAggregationService.Contracts.Responses.ScheduleResponse;

[MessagePackObject]
public class EmployeeAvailabilityScheduleResponse
{
    [Key(0)] public int EmployeeId { get; set; }
    [Key(1)] public DateOnly Date { get; set; }

    [Key(2)] public List<ScheduleAvailabilityResponse> Schedules { get; set; } = [];
}