using MessagePack;
using QAggregationService.Contracts.Requests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.RequestTests;

public class EmployeeReportRequestTests
{
    [Fact]
    public void AggregationRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new EmployeeReportRequest
        {
            EmployeeId = 1,
            FromDate = DateTime.UtcNow.Date.AddHours(5),
            ToDate = DateTime.UtcNow.Date.AddHours(9)
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<EmployeeReportRequest>(bytes);
        
        deserializedRequest.EmployeeId.ShouldBe(originalRequest.EmployeeId);
        deserializedRequest.FromDate.ShouldBe(originalRequest.FromDate);
        deserializedRequest.ToDate.ShouldBe(originalRequest.ToDate);
    }
}