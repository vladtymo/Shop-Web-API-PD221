﻿using BusinessLogic.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductModel>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .Length(10, 4000);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .Matches(@"^[A-Z].*$").WithMessage("{PropertyName} must starts with uppercase letter.");

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.Image)
                .NotNull().WithMessage("{PropertyName} is required.");
        }

        private static bool LinkMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            //Courtesy of @Pure.Krome's comment and https://stackoverflow.com/a/25654227/563532
            Uri outUri;
            return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
