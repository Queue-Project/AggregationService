using MessagePack;
using QContracts.Enums;

namespace QAggregationService.Contracts.Responses.ScheduleResponse;

[MessagePackObject]
public class BookedSlotResponse
{
    [Key(0)] public int QueueId { get; set; }

    [Key(1)] public DateTimeOffset From { get; set; }

    [Key(2)] public DateTimeOffset To { get; set; }

    [Key(3)] public CurrentQueueStatus Status { get; set; }

    [Key(4)] public int CustomerId { get; set; }

    [Key(5)] public string CustomerName { get; set; } = "";

    [Key(6)] public string Email { get; set; } = "";

    [Key(7)] public string PhoneNumber { get; set; } = "";
}