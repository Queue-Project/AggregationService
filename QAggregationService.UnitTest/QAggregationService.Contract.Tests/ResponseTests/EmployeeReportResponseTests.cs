using MessagePack;
using QAggregationService.Contracts.Responses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.ResponseTests;

public class EmployeeReportResponseTests
{
     [Fact]
    public void AggregationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new EmployeeReportResponse
        {
            EmployeeId = 1,
            EmployeeName= "Test Name",
            TotalQueues = 20,
            PendingQueues = 5,
            CompletedQueues = 12,
            CancelledQueues = 2,
            DidNotComeQueues = 1,
            TotalComplaintsReceived = 4,
            PendingComplaints = 1,
            ReviewedComplaints = 1,
            ResolvedComplaints = 1,
            AverageReviewGrade = 4,
            TotalReviewsReceived = 10,
            RecentQueues = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedRequest = MessagePackSerializer.Deserialize<EmployeeReportResponse>(bytes);

        deserializedRequest.EmployeeId.ShouldBe(originalResponse.EmployeeId);
        deserializedRequest.EmployeeName.ShouldBe(originalResponse.EmployeeName);
        deserializedRequest.TotalQueues.ShouldBe(originalResponse.TotalQueues);
        deserializedRequest.PendingQueues.ShouldBe(originalResponse.PendingQueues);
        deserializedRequest.CompletedQueues.ShouldBe(originalResponse.CompletedQueues);
        deserializedRequest.CancelledQueues.ShouldBe(originalResponse.CancelledQueues);
        deserializedRequest.DidNotComeQueues.ShouldBe(originalResponse.DidNotComeQueues);
        deserializedRequest.TotalComplaintsReceived.ShouldBe(originalResponse.TotalComplaintsReceived);
        deserializedRequest.PendingComplaints.ShouldBe(originalResponse.PendingComplaints);
        deserializedRequest.ReviewedComplaints.ShouldBe(originalResponse.ReviewedComplaints);
        deserializedRequest.ResolvedComplaints.ShouldBe(originalResponse.ResolvedComplaints);
        deserializedRequest.AverageReviewGrade.ShouldBe(originalResponse.AverageReviewGrade);
        deserializedRequest.TotalReviewsReceived.ShouldBe(originalResponse.TotalReviewsReceived);
        deserializedRequest.RecentQueues.ShouldBe(originalResponse.RecentQueues);
    }
}