using BranchService.Contracts.Events.Enums;
using BranchService.Contracts.Responses;
using QContracts.Enums;
using QContracts.Responses;
using QUserService.Contracts.Responses.EmployeeResponses;
using RabbitMQ.Client;
using RecommendationService.Contracts.Responses;
using CustomerInfo = QUserService.Contracts.Responses.CustomerResponses.CustomerInfo;
using EmployeeInfo = QUserService.Contracts.Responses.EmployeeResponses.EmployeeInfo;
using BlockedCustomerInfo = QUserService.Contracts.Responses.BlockedCustomersResponses.BlockedCustomerInfo;


namespace QAggregationService.UnitTest.QAggregationService.Application.Tests.Extensions;

public static class TestData
{
    public static List<EmployeeInfo> EmployeeInfos()
    {
        return new List<EmployeeInfo>
        {
            new EmployeeInfo
            {
                EmployeeId = 1,
                CompanyId = 1,
                BranchId = 1,
                CompanyServiceId = 1,
                FirstName = "Test Firstname",
                LastName = "Test Lastname",
                PhoneNumber = "+992923324252",
                Position = "Test Position",
                CreatedAt = DateTime.UtcNow
            },

            new EmployeeInfo
            {
                EmployeeId = 2,
                CompanyId = 1,
                BranchId = 1,
                CompanyServiceId = 1,
                FirstName = "Test Firstname2",
                LastName = "Test Lastname2",
                PhoneNumber = "+992923324211",
                Position = "Test Position2",
                CreatedAt = DateTime.UtcNow
            },

            new EmployeeInfo
            {
                EmployeeId = 3,
                CompanyId = 1,
                BranchId = 1,
                CompanyServiceId = 1,
                FirstName = "Test Firstname2",
                LastName = "Test Lastname2",
                PhoneNumber = "+992923324222",
                Position = "Test Position2",
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    public static List<CustomerInfo> CustomerInfos()
    {
        return new List<CustomerInfo>
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
    }


    public static List<BlockedCustomerInfo> BlockedCustomerInfos()
    {
        return new List<BlockedCustomerInfo>()
        {
            new BlockedCustomerInfo
            {
                CompanyId = 1,
                BlockedId = 1,
                CustomerId = 4,
                DoesBanForever = false,
                BannedUntil = DateTime.UtcNow.Date.AddMonths(1),
                Reason = "Did Not Come 3 times",
                CreatedAt = new DateTime(2026, 06, 20, 09, 00, 00)
            }
        };
    }

    public static List<QueueInfo> QueueInfos()
    {
        return new List<QueueInfo>
        {
            new QueueInfo
            {
                Id = 1,
                CompanyId = 1,
                BranchId = 1,
                ServiceId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Test Customer Name",
                EmployeeName = "Test Employee Name",
                StartTime = new DateTimeOffset(new DateTime(2026, 06, 20, 08, 10, 00)),
                EndTime = new DateTimeOffset(new DateTime(2026, 06, 20, 08, 50, 00)),
                CurrentQueueStatus = CurrentQueueStatus.Completed,
                CancelReason = null,
                CreatedAt = new DateTime(2026, 06, 19, 14, 00, 00)
            },
            new QueueInfo
            {
                Id = 2,
                CompanyId = 1,
                BranchId = 1,
                ServiceId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Test Customer Name",
                EmployeeName = "Test Employee Name",
                StartTime = new DateTimeOffset(new DateTime(2026, 06, 21, 10, 10, 00)),
                EndTime = new DateTimeOffset(new DateTime(2026, 06, 21, 10, 50, 00)),
                CurrentQueueStatus = CurrentQueueStatus.CancelledByEmployee,
                CancelReason = null,
                CreatedAt = new DateTime(2026, 06, 20, 14, 00, 00)
            },
            new QueueInfo
            {
                Id = 3,
                CompanyId = 1,
                BranchId = 1,
                ServiceId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Test Customer Name",
                EmployeeName = "Test Employee Name",
                StartTime = new DateTimeOffset(new DateTime(2026, 06, 22, 11, 10, 00)),
                EndTime = new DateTimeOffset(new DateTime(2026, 06, 22, 11, 50, 00)),
                CurrentQueueStatus = CurrentQueueStatus.Completed,
                CancelReason = null,
                CreatedAt = new DateTime(2026, 06, 21, 14, 00, 00)
            }
        };
    }


    public static List<ComplaintInfo> ComplaintInfos()
    {
        return new List<ComplaintInfo>
        {
            new ComplaintInfo
            {
                Id = 1,
                QueueId = 2,
                CustomerId = 1,
                EmployeeId = 1,
                Status = CurrentComplaintStatus.Pending,
                ComplaintText = "Test Text",
                ResponseText = null,
                CreatedAt = new DateTime(2026, 06, 21, 13, 00, 00)
            }
        };
    }

    public static List<ReviewInfo> ReviewInfos()
    {
        return new List<ReviewInfo>
        {
            new ReviewInfo
            {
                Id = 1,
                QueueId = 1,
                CustomerId = 1,
                EmployeeId = 1,
                Grade = 4,
                ReviewText = "Test Review Text",
                CreatedAt = new DateTime(2026, 06, 20, 13, 00, 00)
            },
            new ReviewInfo
            {
                Id = 2,
                QueueId = 3,
                CustomerId = 1,
                EmployeeId = 1,
                Grade = 5,
                ReviewText = "Test Review Text",
                CreatedAt = new DateTime(2026, 06, 21, 13, 20, 00)
            },
        };
    }

    public static CompanyResponse CompanyResponse()
    {
        return new CompanyResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            CompanyName = "Test Company Name",
            IsValid = true,
            ErrorMessage = null
        };
    }

    public static BranchResponse BranchResponse()
    {
        return new BranchResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            BranchName = "Test Branch Name",
            IsValid = true,
            ErrorMessage = null
        };
    }

    public static CompanyServiceResponse CompanyServiceResponse()
    {
        return new CompanyServiceResponse
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            CompanyServiceId = 1,
            CompanyServiceName = "Test Service Name",
            IsValid = true,
            ErrorMessage = null
        };
    }

    public static List<BranchResponse> CompanyBranches()
    {
        return new List<BranchResponse>
        {
            new BranchResponse
            {
                RequestId = Guid.NewGuid(),
                CompanyId = 1,
                BranchId = 1,
                BranchName = "Test Branch Name",
                IsValid = true,
            },
            new BranchResponse
            {
                RequestId = Guid.NewGuid(),
                CompanyId = 1,
                BranchId = 2,
                BranchName = "Test Branch Name2",
                IsValid = true,
            },
        };
    }


    public static List<CompanyServiceResponse> CompanyServices()
    {
        return new List<CompanyServiceResponse>
        {
            new CompanyServiceResponse
            {
                RequestId = Guid.NewGuid(),
                CompanyId = 1,
                CompanyServiceId = 1,
                CompanyServiceName = "Test Service Name",
                IsValid = true,
                ErrorMessage = null
            },
            new CompanyServiceResponse
            {
                RequestId = Guid.NewGuid(),
                CompanyId = 1,
                CompanyServiceId = 2,
                CompanyServiceName = "Test Service Name2",
                IsValid = true,
                ErrorMessage = null
            }
        };
    }

    public static EmployeeScheduleResponse ScheduleInfo()
    {
        return new EmployeeScheduleResponse
        {
            EmployeeId = 1,
            Date = new DateOnly(2026, 07, 13),
            Schedules = new List<EmployeeScheduleInfo>
            {
                new EmployeeScheduleInfo
                {
                    ScheduleId = 1,
                    Description = "Before lunch time working hours",
                    AvailableSlots = new List<TimeSlot>
                    {
                        new TimeSlot
                        {
                            From = DateTimeOffset.UtcNow,
                            To = DateTimeOffset.UtcNow.AddHours(1)
                        }
                    }
                }
            }
        };
    }

    public static EmployeeScheduleResponse ScheduleInfoWithEmptySlots()
    {
        return new EmployeeScheduleResponse
        {
            EmployeeId = 1,
            Date = new DateOnly(2026, 07, 13),
            Schedules = new List<EmployeeScheduleInfo>()
        };
    }

    public static PagedResponse<RecommendedCompanyResponse> CompanyExpectedResponse()
    {
        return new PagedResponse<RecommendedCompanyResponse>
        {
            Items = new List<RecommendedCompanyResponse>
            {
                new RecommendedCompanyResponse
                {
                    CompanyId = 1,
                    CategoryId = 1,
                    RecommendationScore = 20,
                    AverageRating = 4.5,
                    ReviewCount = 4,
                    ComplaintCount = 0,
                    CompletedQueues = 10,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            },
            PageNumber = 1,
            PageSize = 15,
            TotalCount = 1
        };
    }

    public static List<CompanyDetailsResponse> CompanyDetailsResponses()
    {
        return new List<CompanyDetailsResponse>
        {
            new CompanyDetailsResponse
            {
                CompanyId = 1,
                CompanyCategory = CompanyCategory.Healthcare,
                CompanyName = "Test Name",
                Address = "Test Address",
                EmailAddress = "TestEmail@gmail.com",
                PhoneNumber = "+992923324252"
            }
        };
    }

    public static PagedResponse<RecommendedBranchResponse> BranchExpectedResponse()
    {
        return new PagedResponse<RecommendedBranchResponse>
        {
            Items = new List<RecommendedBranchResponse>
            {
                new RecommendedBranchResponse
                {
                    BranchId = 1,
                    CompanyId = 1,
                    CategoryId = 1,
                    RecommendationScore = 20,
                    AverageRating = 4.5,
                    ReviewCount = 4,
                    ComplaintCount = 0,
                    CompletedQueues = 10,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            },
            PageNumber = 1,
            PageSize = 15,
            TotalCount = 1
        };
    }

    public static List<BranchDetailsResponse> BranchDetailsResponses()
    {
        return new List<BranchDetailsResponse>
        {
            new BranchDetailsResponse
            {
                BranchId = 1,
                CompanyId = 1,
                BranchName = "Test Branch Name",
                CompanyName = "Test Name",
                Address = "Test Address",
                PhoneNumber = "+992923324252",
                IsActive = true
            }
        };
    }

    public static PagedResponse<RecommendedServiceResponse> ServiceExpectedResponse()
    {
        return new PagedResponse<RecommendedServiceResponse>
        {
            Items = new List<RecommendedServiceResponse>
            {
                new RecommendedServiceResponse
                {
                    ServiceId = 1,
                    BranchId = 1,
                    CompanyId = 1,
                    CategoryId = 1,
                    RecommendationScore = 20,
                    AverageRating = 4.5,
                    ReviewCount = 4,
                    ComplaintCount = 0,
                    CompletedQueues = 10,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            },
            PageNumber = 1,
            PageSize = 15,
            TotalCount = 1
        };
    }

    public static List<ServiceDetailsResponse> ServiceDetailsResponses()
    {
        return new List<ServiceDetailsResponse>
        {
            new ServiceDetailsResponse
            {
                ServiceId = 1,
                BranchId = 1,
                CompanyId = 1,
                BranchName = "Test Branch Name",
                CompanyName = "Test Name",
                ServiceName = "Test Service Name",
                Description = "Test Description",
                ServiceDuration = 30
            }
        };
    }

    public static PagedResponse<RecommendedEmployeeResponse> EmployeeExpectedResponse()
    {
        return new PagedResponse<RecommendedEmployeeResponse>
        {
            Items = new List<RecommendedEmployeeResponse>
            {
                new RecommendedEmployeeResponse
                {
                    EmployeeId = 1,
                    ServiceId = 1,
                    BranchId = 1,
                    CompanyId = 1,
                    CategoryId = 1,
                    RecommendationScore = 20,
                    AverageRating = 4.5,
                    ReviewCount = 4,
                    ComplaintCount = 0,
                    CompletedQueues = 10,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            },
            PageNumber = 1,
            PageSize = 15,
            TotalCount = 1
        };
    }

    public static List<EmployeeDetailsResponse> EmployeeDetailsResponses()
    {
        return new List<EmployeeDetailsResponse>
        {
            new EmployeeDetailsResponse
            {
                EmployeeId = 1,
                CompanyServiceId = 1,
                BranchId = 1,
                CompanyId = 1,
                FirstName = "Test FN",
                LastName = "Test LN",
                Position = "Test Position",
                EmailAddress = "test@gmail.com",
                PhoneNumber = "+992934233242"
            }
        };
    }
}