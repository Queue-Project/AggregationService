using MessagePack;
using QAggregationService.Contracts.Responses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.ResponseTests;

public class CustomerReportResponseTests
{
    [Fact]
    public void AggregationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new CustomerReportResponse()
        {
            CustomerId = 1,
            CustomerName = "Test Name",
            TotalQueues = 20,
            PendingQueues = 5,
            CompletedQueues = 12,
            CancelledQueues = 2,
            DidNotComeQueues = 1,
            TotalComplaints = 4,
            PendingComplaints = 1,
            ReviewedComplaints = 1,
            ResolvedComplaints = 1,
            AverageReviewGrade = 4,
            TotalReviews = 10,
            RecentQueues = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedRequest = MessagePackSerializer.Deserialize<CustomerReportResponse>(bytes);

        deserializedRequest.CustomerId.ShouldBe(originalResponse.CustomerId);
        deserializedRequest.CustomerName.ShouldBe(originalResponse.CustomerName);
        deserializedRequest.TotalQueues.ShouldBe(originalResponse.TotalQueues);
        deserializedRequest.PendingQueues.ShouldBe(originalResponse.PendingQueues);
        deserializedRequest.CompletedQueues.ShouldBe(originalResponse.CompletedQueues);
        deserializedRequest.CancelledQueues.ShouldBe(originalResponse.CancelledQueues);
        deserializedRequest.DidNotComeQueues.ShouldBe(originalResponse.DidNotComeQueues);
        deserializedRequest.TotalComplaints.ShouldBe(originalResponse.TotalComplaints);
        deserializedRequest.PendingComplaints.ShouldBe(originalResponse.PendingComplaints);
        deserializedRequest.ReviewedComplaints.ShouldBe(originalResponse.ReviewedComplaints);
        deserializedRequest.ResolvedComplaints.ShouldBe(originalResponse.ResolvedComplaints);
        deserializedRequest.AverageReviewGrade.ShouldBe(originalResponse.AverageReviewGrade);
        deserializedRequest.TotalReviews.ShouldBe(originalResponse.TotalReviews);
        deserializedRequest.RecentQueues.ShouldBe(originalResponse.RecentQueues);
    }
}