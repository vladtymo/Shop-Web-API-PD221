using Core.Entities;
using FluentValidation;

namespace Core.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .Matches(@"^[A-Z].*$").WithMessage("{PropertyName} must starts with uppercase letter.");
        }
    }
}
