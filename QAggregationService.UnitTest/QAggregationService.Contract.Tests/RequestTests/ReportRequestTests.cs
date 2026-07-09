using MessagePack;
using QAggregationService.Contracts.Requests;
using QContracts.Enums;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.RequestTests;

public class ReportRequestTests
{
    [Fact]
    public void AggregationRequest_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalRequest = new ReportRequest()
        {
            CompanyId = 1,
            BranchId = 1,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            FromDate = DateTime.UtcNow.Date.AddHours(5),
            ToDate = DateTime.UtcNow.Date.AddHours(9),
            ComplaintStatus = CurrentComplaintStatus.Pending,
            QueueStatus = CurrentQueueStatus.Confirmed
        };

        var bytes = MessagePackSerializer.Serialize(originalRequest);
        var deserializedRequest = MessagePackSerializer.Deserialize<ReportRequest>(bytes);

        deserializedRequest.CompanyId.ShouldBe(originalRequest.CompanyId);
        deserializedRequest.BranchId.ShouldBe(originalRequest.BranchId);
        deserializedRequest.ServiceId.ShouldBe(originalRequest.ServiceId);
        deserializedRequest.PageNumber.ShouldBe(originalRequest.PageNumber);
        deserializedRequest.PageSize.ShouldBe(originalRequest.PageSize);
        deserializedRequest.FromDate.ShouldBe(originalRequest.FromDate);
        deserializedRequest.ToDate.ShouldBe(originalRequest.ToDate);
        deserializedRequest.ComplaintStatus.ShouldBe(originalRequest.ComplaintStatus);
        deserializedRequest.QueueStatus.ShouldBe(originalRequest.QueueStatus);
    }
}