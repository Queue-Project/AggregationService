using MessagePack;
using QAggregationService.Contracts.Requests;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.RequestTests;

public class CustomerReportRequestTests
{
    [Fact]
    public void AggregationRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new CustomerReportRequest
        {
            CustomerId = 1,
            FromDate = DateTime.UtcNow.Date.AddHours(5),
            ToDate = DateTime.UtcNow.Date.AddHours(9)
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<CustomerReportRequest>(bytes);
        
        deserializedRequest.CustomerId.ShouldBe(originalRequest.CustomerId);
        deserializedRequest.FromDate.ShouldBe(originalRequest.FromDate);
        deserializedRequest.ToDate.ShouldBe(originalRequest.ToDate);
    }
}