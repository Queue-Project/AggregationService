using System.Net;
using BranchService.Contracts.Interfaces;
using BranchService.Contracts.Requests;
using BranchService.Contracts.Responses;
using MagicOnion;
using Microsoft.Extensions.Logging;
using Moq;
using QAggregationService.Application.Caching;
using QAggregationService.Application.Exceptions;
using QAggregationService.Application.Services;
using QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;
using QContracts.Interfaces;
using QContracts.Responses;
using QUserService.Contracts.Interfaces;
using Shouldly;
using BlockedCustomerInfo = QUserService.Contracts.Responses.BlockedCustomersResponses.BlockedCustomerInfo;
using EmployeeInfo = QUserService.Contracts.Responses.EmployeeResponses.EmployeeInfo;

namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.AggregationServiceTests;

public class GetCompanyDashboardTests
{
    private readonly Mock<IQueueService> _mockQueueService;
    private readonly Mock<IBranchService> _mockBranchService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<AggregationService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMemoryCacheService> _mockMemoryCacheService;
    private readonly AggregationService _aggregationService;

    public GetCompanyDashboardTests()
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
    public async Task Service_Should_Return_Company_Dashboard_Successfully()
    {
        int companyId = 1;
        
        var expectedCompanyResponse = TestData.CompanyResponse();

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));


        var expectedCompanyBranches = TestData.CompanyBranches();
        _mockBranchService.Setup(s => s.GetCompanyBranches(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyBranches));

        var expectedCompanyServices = TestData.CompanyServices();
        _mockBranchService.Setup(s => s.GetCompanyServices(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyServices));
        
        var expectedCompanyEmployeesResponse = TestData.EmployeeInfos();
        _mockUserService.Setup(s => s.GetAllCompanyEmployees(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyEmployeesResponse));
        
        
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
        
        var expectedCompanyBlockedCustomersResponse = TestData.BlockedCustomerInfos();

        _mockUserService.Setup(s => s.GetAllCompanyBlockedCustomers(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyBlockedCustomersResponse));
        
        _mockMemoryCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<BranchResponse>>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<BranchResponse>>> factory, TimeSpan absoluteExpiration) =>
            {
                return factory().Result;
            });
        
        _mockMemoryCacheService.Setup(s => s.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<CompanyServiceResponse>>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<CompanyServiceResponse>>> factory, TimeSpan absoluteExpiration) =>
            {
                return factory().Result;
            });
        
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
                It.IsAny<Func<Task<List<BlockedCustomerInfo>>>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((string key, Func<Task<List<BlockedCustomerInfo>>> factory, TimeSpan absoluteExpiration,
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

        
        
      
        
        
        var expectedCompanyQueuesResponse = TestData.QueueInfos();

        _mockQueueService.Setup(s => s.GetCompanyQueuesAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyQueuesResponse));

        var expectedCompanyReviewResponse = TestData.ReviewInfos();

        _mockQueueService.Setup(s => s.GetCompanyReviewsAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyReviewResponse));

        var expectedCompanyComplaintsResponse = TestData.ComplaintInfos();

        _mockQueueService.Setup(s => s.GetCompanyComplaintsAsync(companyId))
            .Returns(UnaryResult.FromResult(expectedCompanyComplaintsResponse));

        
        //Act
        var result = await _aggregationService.GetCompanyDashboard(companyId);
        
        //Assert
        
        result.CompanyId.ShouldBe(companyId);
        result.CompanyName.ShouldBe(expectedCompanyResponse.CompanyName);
        result.TotalComplaints.ShouldBe(1);
        result.TotalBlockedCustomers.ShouldBe(1);
        result.TotalQueues.ShouldBe(3);
        result.TotalReviews.ShouldBe(2);
        result.TotalBranches.ShouldBe(2);
        result.TotalCustomers.ShouldBe(3);
        result.TotalEmployees.ShouldBe(3);
        result.TotalServices.ShouldBe(2);
        result.TotalPendingComplaints.ShouldBe(1);
        result.TotalResolvedComplaints.ShouldBe(0);
        result.TotalReviewedComplaints.ShouldBe(0);
        result.TopBranchQueueCount.ShouldBe(3);
    }
    
    [Fact]
    public async Task Service_Should_Throw_When_Company_Not_Found()
    {
        //Arrange
        int companyId = 1;
        
        var expectedCompanyResponse = TestData.CompanyResponse();
        expectedCompanyResponse.IsValid = false;
        expectedCompanyResponse.ErrorMessage = $"Company with Id {companyId} not found";

        _mockBranchService.Setup(s => s.CheckCompanyId(It.IsAny<CompanyRequest>()))
            .Returns(UnaryResult.FromResult(expectedCompanyResponse));
        
          
        //Act
        var result =  _aggregationService.GetCompanyDashboard(companyId);
        
        //

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe(expectedCompanyResponse.ErrorMessage);

    }

}