using MessagePack;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.ResponseTests;

public class QueueReportItemResponseTests
{
    [Fact]
    public void AggregationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new QueueReportItem()
        {
            Id = 1,
            CustomerName = "Test Name",
            EmployeeName = "Test Name",
            Status = "Pending",
            StartTime = DateTimeOffset.UtcNow.Date.AddHours(5),
            EndTime = DateTimeOffset.UtcNow.Date.AddHours(6)
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedRequest = MessagePackSerializer.Deserialize<QueueReportItem>(bytes);

        deserializedRequest.Id.ShouldBe(originalResponse.Id);
        deserializedRequest.CustomerName.ShouldBe(originalResponse.CustomerName);
        deserializedRequest.EmployeeName.ShouldBe(originalResponse.EmployeeName);
        deserializedRequest.Status.ShouldBe(originalResponse.Status);
        deserializedRequest.StartTime.ShouldBe(originalResponse.StartTime);
        deserializedRequest.EndTime.ShouldBe(originalResponse.EndTime);
    }
}