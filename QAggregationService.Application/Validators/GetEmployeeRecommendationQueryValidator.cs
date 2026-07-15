using FluentValidation;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetEmployeeRecommendations;

namespace QAggregationService.Application.Validators;

public class GetEmployeeRecommendationQueryValidator: AbstractValidator<GetEmployeeRecommendationQuery>
{
    public GetEmployeeRecommendationQueryValidator()
    {
        RuleFor(s => s.CompanyId).NotEmpty().WithMessage("CompanyId is required");
        RuleFor(s => s.BranchId).NotEmpty().WithMessage("BranchId is required");
        RuleFor(s => s.ServiceId).NotEmpty().WithMessage("ServiceId is required");

        RuleFor(s => s.PageNumber).NotEmpty().WithMessage("PageNumber is required").GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");
    }
}