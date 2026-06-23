using System.Net;
using BranchService.Contracts.Interfaces;
using BranchService.Contracts.Requests;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Exceptions;
using QAggregationService.Application.Services;
using QAggregationService.Contracts.Requests;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QContracts.Enums;
using QContracts.Interfaces;
using QContracts.Responses;
using QUserService.Contracts.Interfaces;
using Shouldly;
using BlockedCustomerInfo = QUserService.Contracts.Responses.BlockedCustomersResponses.BlockedCustomerInfo;
using EmployeeInfo = QUserService.Contracts.Responses.EmployeeResponses.EmployeeInfo;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.AggregationServiceTests;

public class GetReportAsyncTests
{
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<AggregationService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;
    private readonly AggregationService _aggregationService;

    public GetReportAsyncTests()
    {
        _mockQueueService = new Mock<IQueueService>();
        _mockBranchService = new Mock<IBranchService>();
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<AggregationService>>();
        _mockCacheService = new Mock<ICacheService>();
        _mockMemoryCacheService = new Mock<IMemoryCacheService>();
        _aggregationService = new AggregationService(_mockQueueService.Object, _mockBranchService.Object,
            _mockLogger.Object, _mockCacheService.Object, _mockMemoryCacheService.Object, _mockUserService.Object);
    }

    [Fact]
    public async Task Service_Should_Return_Company_Report_Successfully()
    {
        //Arrange
        var request = new ReportRequest()
        {
            CompanyId = 1,
            BranchId = 1,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            QueueStatus = CurrentQueueStatus.Completed,
            ComplaintStatus = CurrentComplaintStatus.Pending,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };

        int companyId = 1;
        
        var expectedCompanyResponse = TestData.CompanyResponse();

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));

        var expectedBranchResponse = TestData.BranchResponse();

        _mockBranchService.Setup(s => s.CheckBranchId(It.IsAny<BranchRequest>()))
            .Returns(UnaryResult.FromResult(expectedBranchResponse));

        var expectedCompanyServiceResponse = TestData.CompanyServiceResponse();
        _mockBranchService.Setup(s => s.CheckCompanyServiceId(It.IsAny<CompanyServiceRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyServiceResponse));

        var expectedCompanyQueuesResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetCompanyQueuesAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyQueuesResponse));

        var expectedCompanyReviewResponse = TestData.ReviewInfos();

        _mockQueueService.Setup(s => s.GetCompanyReviewsAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyReviewResponse));

        var expectedCompanyComplaintsResponse = TestData.ComplaintInfos();

        _mockQueueService.Setup(s => s.GetCompanyComplaintsAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyComplaintsResponse));

        var expectedCompanyCustomersResponse = new List<CustomerInfo>
        {
            new CustomerInfo
            {
                CustomerId = 1,
                FirstName = "Test Firstname",
                LastName = "Test Lastname",
               
                CreatedAt = DateTime.UtcNow
            },

            new CustomerInfo()
            {
                CustomerId = 2,
                FirstName = "Test Firstname2",
                LastName = "Test Lastname2",
                CreatedAt = DateTime.UtcNow
            },

            new CustomerInfo()
            {
                CustomerId = 3,
                FirstName = "Test Firstname2",
                LastName = "Test Lastname2",
                CreatedAt = DateTime.UtcNow
            }
        };

        _mockQueueService.Setup(s => s.GetAllCompanyCustomers(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyCustomersResponse));


        var expectedCompanyEmployeesResponse = TestData.EmployeeInfos();
        _mockUserService.Setup(s => s.GetAllCompanyEmployees(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyEmployeesResponse));

        var expectedCompanyBlockedCustomersResponse = TestData.BlockedCustomerInfos();

        _mockUserService.Setup(s => s.GetAllCompanyBlockedCustomers(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyBlockedCustomersResponse));
        
        
        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<CustomerInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<CustomerInfo>>> factory, TimeSpan absoluteExpiration,
                TimeSpan slidingExpiration) =>
            {
                return factory().Result;
            });
        
        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<EmployeeInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<EmployeeInfo>>> factory, TimeSpan absoluteExpiration,
                TimeSpan slidingExpiration) =>
            {
                return factory().Result;
            });
        
        _mockCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<BlockedCustomerInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<BlockedCustomerInfo>>> factory, TimeSpan absoluteExpiration,
                TimeSpan slidingExpiration) =>
            {
                return factory().Result;
            });
        
        
        //Act
        var result = await _aggregationService.GetReportAsync(request);
        
        //Assert
        
        result.CompanyId.ShouldBe(1);
        result.CompanyName.ShouldBe(expectedCompanyResponse.CompanyName);
        result.PageNumber.ShouldBe(request.PageNumber);
        result.PageSize.ShouldBe(result.PageSize);
        result.PendingComplaints.ShouldBe(0);
        result.TotalBlockedCustomers.ShouldBe(1);
        result.TotalComplaints.ShouldBe(0);
        result.TotalReviews.ShouldBe(2);
        result.FiveStarReviews.ShouldBe(1);
        result.FourStarReviews.ShouldBe(1);
        result.ThreeStarReviews.ShouldBe(0);
        result.TwoStarReviews.ShouldBe(0);
        result.OneStarReviews.ShouldBe(0);
        result.CompletedQueues.ShouldBe(2);
        result.TotalRecords.ShouldBe(2);
        result.TotalQueues.ShouldBe(2);

    }
    
    [Fact]
    public async Task Service_Should_Throw_When_CompanyId_Is_Null()
    {
        //Arrange
        var request = new ReportRequest()
        {
            CompanyId = null,
            BranchId = 1,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            QueueStatus = CurrentQueueStatus.Completed,
            ComplaintStatus = CurrentComplaintStatus.Pending,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };
        
          
        //Act
        var result =  _aggregationService.GetReportAsync(request);
        
        //

        var exception = await result.ShouldThrowAsync<ArgumentException>();
    
        exception.Message.ShouldBe("Company is required");

    }

    [Fact]
    public async Task Service_Should_Throw_When_Company_Not_Found()
    {
        //Arrange
        var request = new ReportRequest()
        {
            CompanyId = 10,
            BranchId = 1,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            QueueStatus = CurrentQueueStatus.Completed,
            ComplaintStatus = CurrentComplaintStatus.Pending,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };
        
        var expectedCompanyResponse = TestData.CompanyResponse();
        expectedCompanyResponse.IsValid = false;
        expectedCompanyResponse.ErrorMessage = $"Company with Id {request.CompanyId} not found";

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));
        
          
        //Act
        var result =  _aggregationService.GetReportAsync(request);
        
        //

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe(expectedCompanyResponse.ErrorMessage);

    }
    
    
    [Fact]
    public async Task Service_Should_Throw_When_Branch_Not_Found()
    {
        //Arrange
        var request = new ReportRequest()
        {
            CompanyId = 1,
            BranchId = 10,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            QueueStatus = CurrentQueueStatus.Completed,
            ComplaintStatus = CurrentComplaintStatus.Pending,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };
        
        var expectedCompanyResponse = TestData.CompanyResponse();
       

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));
        
        var expectedBranchResponse = TestData.BranchResponse();
        expectedBranchResponse.IsValid = false;
        expectedBranchResponse.ErrorMessage = $"Branch with Id {request.BranchId} not found";

        _mockBranchService.Setup(s => s.CheckBranchId(It.IsAny<BranchRequest>()))
            .Returns(UnaryResult.FromResult(expectedBranchResponse));
        
          
        //Act
        var result =  _aggregationService.GetReportAsync(request);
        
        //

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe(expectedBranchResponse.ErrorMessage);

    }
    
    [Fact]
    public async Task Service_Should_Throw_When_Company_Service_Not_Found()
    {
        //Arrange
        var request = new ReportRequest()
        {
            CompanyId = 1,
            BranchId = 10,
            ServiceId = 1,
            PageNumber = 1,
            PageSize = 15,
            QueueStatus = CurrentQueueStatus.Completed,
            ComplaintStatus = CurrentComplaintStatus.Pending,
            FromDate = new DateTime(2026, 06, 06),
            ToDate = new DateTime(2026, 06, 23)
        };
        
        var expectedCompanyResponse = TestData.CompanyResponse();
       

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));
        
        var expectedBranchResponse = TestData.BranchResponse();
        
        _mockBranchService.Setup(s => s.CheckBranchId(It.IsAny<BranchRequest>()))
            .Returns(UnaryResult.FromResult(expectedBranchResponse));
        
        var expectedCompanyServiceResponse = TestData.CompanyServiceResponse();
        expectedCompanyServiceResponse.IsValid = false;
        expectedCompanyServiceResponse.ErrorMessage = $"Company service with Id {request.ServiceId} not found";
        
        _mockBranchService.Setup(s => s.CheckCompanyServiceId(It.IsAny<CompanyServiceRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyServiceResponse));
          
        //Act
        var result =  _aggregationService.GetReportAsync(request);
        
        //

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe(expectedCompanyServiceResponse.ErrorMessage);

    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}