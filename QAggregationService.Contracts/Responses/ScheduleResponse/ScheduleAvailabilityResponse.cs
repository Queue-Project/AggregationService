using MessagePack;

namespace QAggregationService.Contracts.Responses.ScheduleResponse;

[MessagePackObject]
public class ScheduleAvailabilityResponse
{
    [Key(0)] public int ScheduleId { get; set; }

    [Key(1)] public string? Description { get; set; }

    [Key(2)] public List<AvailableSlotResponse> AvailableSlots { get; set; } = [];

    [Key(3)] public List<BookedSlotResponse> BookedSlots { get; set; } = [];
}