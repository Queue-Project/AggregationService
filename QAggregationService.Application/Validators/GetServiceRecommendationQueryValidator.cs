using FluentValidation;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetServiceRecommendations;

namespace QAggregationService.Application.Validators;

public class GetServiceRecommendationQueryValidator : AbstractValidator<GetServiceRecommendationQuery>
{
    public GetServiceRecommendationQueryValidator()
    {
        RuleFor(s => s.CompanyId).NotEmpty().WithMessage("CompanyId is required");
        RuleFor(s => s.BranchId).NotEmpty().WithMessage("BranchId is required");

        RuleFor(s => s.PageNumber).NotEmpty().WithMessage("PageNumber is required").GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");
    }
}