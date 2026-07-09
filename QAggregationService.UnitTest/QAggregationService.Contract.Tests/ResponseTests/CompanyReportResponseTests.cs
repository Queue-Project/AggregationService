using MessagePack;
using QAggregationService.Contracts.Responses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.ResponseTests;

public class CompanyReportResponseTests
{
    [Fact]
    public void AggregationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new CompanyReportResponse
        {
            CompanyId = 1,
            CompanyName = "Test Name",
            ReportDate = DateTime.UtcNow,
            TotalQueues = 50,
            PendingQueues = 5,
            ConfirmedQueues = 15,
            CompletedQueues = 30,
            CancelledQueues = 0,
            DidNotComeQueues = 0,
            TotalComplaints = 4,
            PendingComplaints = 1,
            ReviewedComplaints = 1,
            ResolvedComplaints = 1,
            AverageRating = 4,
            TotalReviews = 30,
            FiveStarReviews = 10,
            FourStarReviews = 10,
            ThreeStarReviews = 6,
            TwoStarReviews = 4,
            OneStarReviews = 0,
            TotalEmployees = 10,
            TotalCustomers = 40,
            TotalBlockedCustomers = 1,
            PageNumber = 1,
            PageSize = 15,
            TotalRecords = 50,
            TotalPages = 3,
            Queues = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedRequest = MessagePackSerializer.Deserialize<CompanyReportResponse>(bytes);

        deserializedRequest.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedRequest.CompanyName.ShouldBe(originalResponse.CompanyName);
        deserializedRequest.ReportDate.ShouldBe(originalResponse.ReportDate);
        deserializedRequest.TotalQueues.ShouldBe(originalResponse.TotalQueues);
        deserializedRequest.PendingQueues.ShouldBe(originalResponse.PendingQueues);
        deserializedRequest.ConfirmedQueues.ShouldBe(originalResponse.ConfirmedQueues);
        deserializedRequest.CompletedQueues.ShouldBe(originalResponse.CompletedQueues);
        deserializedRequest.CancelledQueues.ShouldBe(originalResponse.CancelledQueues);
        deserializedRequest.DidNotComeQueues.ShouldBe(originalResponse.DidNotComeQueues);
        deserializedRequest.TotalComplaints.ShouldBe(originalResponse.TotalComplaints);
        deserializedRequest.PendingComplaints.ShouldBe(originalResponse.PendingComplaints);
        deserializedRequest.ReviewedComplaints.ShouldBe(originalResponse.ReviewedComplaints);
        deserializedRequest.ResolvedComplaints.ShouldBe(originalResponse.ResolvedComplaints);
        deserializedRequest.AverageRating.ShouldBe(originalResponse.AverageRating);
        deserializedRequest.TotalReviews.ShouldBe(originalResponse.TotalReviews);
        deserializedRequest.FiveStarReviews.ShouldBe(originalResponse.FiveStarReviews);
        deserializedRequest.FourStarReviews.ShouldBe(originalResponse.FourStarReviews);
        deserializedRequest.ThreeStarReviews.ShouldBe(originalResponse.ThreeStarReviews);
        deserializedRequest.TwoStarReviews.ShouldBe(originalResponse.TwoStarReviews);
        deserializedRequest.OneStarReviews.ShouldBe(originalResponse.OneStarReviews);
        deserializedRequest.TotalEmployees.ShouldBe(originalResponse.TotalEmployees);
        deserializedRequest.TotalCustomers.ShouldBe(originalResponse.TotalCustomers);
        deserializedRequest.TotalBlockedCustomers.ShouldBe(originalResponse.TotalBlockedCustomers);
        deserializedRequest.TotalRecords.ShouldBe(originalResponse.TotalRecords);
        deserializedRequest.PageNumber.ShouldBe(originalResponse.PageNumber);
        deserializedRequest.PageSize.ShouldBe(originalResponse.PageSize);
        deserializedRequest.TotalPages.ShouldBe(originalResponse.TotalPages);
        deserializedRequest.Queues.ShouldBe(originalResponse.Queues);
    }
}