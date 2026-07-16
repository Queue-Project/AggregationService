using FluentValidation;
using QAggregationService.Application.UseCases.Queries.RecommendationService.GetCompanyRecommendations;

namespace QAggregationService.Application.Validators;

public class GetCompanyRecommendationQueryValidator: AbstractValidator<GetCompanyRecommendationQuery>
{
    public GetCompanyRecommendationQueryValidator()
    {
        RuleFor(s => s.CategoryId).NotEmpty().WithMessage("CompanyId is required");
        RuleFor(s => s.PageNumber).NotEmpty().WithMessage("PageNumber is required").GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");
    }
}