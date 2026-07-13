using MessagePack;

namespace QAggregationService.Contracts.Responses.ScheduleResponse;

[MessagePackObject]
public class AvailableSlotResponse
{
    [Key(0)] public DateTimeOffset From { get; set; }
    [Key(1)] public DateTimeOffset To { get; set; }
}