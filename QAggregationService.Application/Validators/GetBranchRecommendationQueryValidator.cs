using FluentValidation;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetBranchRecommendations;

namespace QAggregationService.Application.Validators;

public class GetBranchRecommendationQueryValidator: AbstractValidator<GetBranchRecommendationQuery>
{
    public GetBranchRecommendationQueryValidator()
    {
        RuleFor(s => s.CompanyId).NotEmpty().WithMessage("CompanyId is required");
        RuleFor(s => s.PageNumber).NotEmpty().WithMessage("PageNumber is required").GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");
    }
}