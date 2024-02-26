using Core.DTOs;
using FluentValidation;

namespace Core.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Birthdate)
                .NotEmpty()
                .LessThan(DateTime.Now).WithMessage("Birthdate cannot be future date.");
        }
    }
}
