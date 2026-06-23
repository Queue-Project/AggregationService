using QContracts.Enums;
using QContracts.Responses;
using CustomerInfo = QUserService.Contracts.Responses.CustomerResponses.CustomerInfo;
using EmployeeInfo = QUserService.Contracts.Responses.EmployeeResponses.EmployeeInfo;


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
                 CreatedAt   = new DateTime(2026, 06, 20, 13, 00, 00)
            },
            new ReviewInfo
            {
                Id = 2,
                QueueId = 3,
                CustomerId = 1,
                EmployeeId = 1,
                Grade = 5,
                ReviewText = "Test Review Text",
                CreatedAt   = new DateTime(2026, 06, 21, 13, 20, 00)
            },
        };
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}