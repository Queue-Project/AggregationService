using MessagePack;
using QAggregationService.Contracts.Responses;
using Shouldly;

namespace QAggregationService.UnitTest.QAggregationService.Contract.Tests.ResponseTests;

public class DashboardResponseTests
{
      [Fact]
    public void AggregationResponse_ShouldSerializeAndDeserializeCorrectly()
    {
        var originalResponse = new DashboardResponse()
        {
            CompanyId = 1,
            CompanyName= "Test Name",
            ReportDate = DateTime.UtcNow,
            TotalBranches = 3,
            TotalServices = 10,
            TotalCustomers = 100,
            TotalEmployees = 20,
            TotalBlockedCustomers = 3,
            TotalQueues = 20,
            PendingQueues = 5,
            CompletedQueues = 12,
            CancelledQueues = 2,
            DidNotComeQueues = 1,
            TotalReviews = 10,
            TotalComplaints = 5,
            TotalPendingComplaints = 1,
            TotalReviewedComplaints = 1,
            TotalResolvedComplaints = 1,
            TopServiceName = "Test Name",
            TopServiceQueueCount = 10,
            TopBranchName = "Test Name",
            TopBranchQueueCount = 25,
            TopEmployeeName = "Test Name",
            TopEmployeeQueueCount = 10,
            TopCustomerName = "Test Name",
            TopCustomerQueueCount = 8,
            RecentQueues = null
        };

        var bytes = MessagePackSerializer.Serialize(originalResponse);
        var deserializedRequest = MessagePackSerializer.Deserialize<DashboardResponse>(bytes);

        deserializedRequest.CompanyId.ShouldBe(originalResponse.CompanyId);
        deserializedRequest.CompanyName.ShouldBe(originalResponse.CompanyName);
        deserializedRequest.ReportDate.ShouldBe(originalResponse.ReportDate);
        deserializedRequest.TotalBranches.ShouldBe(originalResponse.TotalBranches);
        deserializedRequest.TotalServices.ShouldBe(originalResponse.TotalServices);
        deserializedRequest.TotalCustomers.ShouldBe(originalResponse.TotalCustomers);
        deserializedRequest.TotalEmployees.ShouldBe(originalResponse.TotalEmployees);
        deserializedRequest.TotalBlockedCustomers.ShouldBe(originalResponse.TotalBlockedCustomers);
        deserializedRequest.TotalQueues.ShouldBe(originalResponse.TotalQueues);
        deserializedRequest.PendingQueues.ShouldBe(originalResponse.PendingQueues);
        deserializedRequest.CompletedQueues.ShouldBe(originalResponse.CompletedQueues);
        deserializedRequest.CancelledQueues.ShouldBe(originalResponse.CancelledQueues);
        deserializedRequest.DidNotComeQueues.ShouldBe(originalResponse.DidNotComeQueues);
        deserializedRequest.TotalReviews.ShouldBe(originalResponse.TotalReviews);
        deserializedRequest.TotalComplaints.ShouldBe(originalResponse.TotalComplaints);
        deserializedRequest.TotalPendingComplaints.ShouldBe(originalResponse.TotalPendingComplaints);
        deserializedRequest.TotalReviewedComplaints.ShouldBe(originalResponse.TotalReviewedComplaints);
        deserializedRequest.TotalResolvedComplaints.ShouldBe(originalResponse.TotalResolvedComplaints);
        deserializedRequest.TopServiceName.ShouldBe(originalResponse.TopServiceName);
        deserializedRequest.TopServiceQueueCount.ShouldBe(originalResponse.TopServiceQueueCount);
        deserializedRequest.TopBranchName.ShouldBe(originalResponse.TopBranchName);
        deserializedRequest.TopBranchQueueCount.ShouldBe(originalResponse.TopBranchQueueCount);
        deserializedRequest.TopEmployeeName.ShouldBe(originalResponse.TopEmployeeName);
        deserializedRequest.TopEmployeeQueueCount.ShouldBe(originalResponse.TopEmployeeQueueCount);
        deserializedRequest.TopCustomerName.ShouldBe(originalResponse.TopCustomerName);
        deserializedRequest.TopCustomerQueueCount.ShouldBe(originalResponse.TopCustomerQueueCount);
        deserializedRequest.RecentQueues.ShouldBe(originalResponse.RecentQueues);
    }
}