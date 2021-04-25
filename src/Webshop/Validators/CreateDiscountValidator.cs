using FluentValidation;
using Webshop.Domain.DTOs.Discount;

namespace Webshop.Validators
{
    public class CreateDiscountValidator : AbstractValidator<CreateDiscount>
    {
        public CreateDiscountValidator()
        {
            RuleFor(discount => discount.Percentage)
                .InclusiveBetween(0, 100)
                .WithMessage("The percentage is outside the valid range 0 - 100");

            RuleFor(discount => discount.MaxQuantity)
                .GreaterThanOrEqualTo(_ => _.MinQuantity)
                .GreaterThan(0)
                .WithMessage("The max quantity must be greater than 0 and than the minimum quantity");

            RuleFor(discount => discount.MinQuantity)
                .LessThanOrEqualTo(_ => _.MaxQuantity)
                .GreaterThan(0)
                .WithMessage("The min quantity must be greater than 0 and less than the maximum quantity");

            RuleFor(discount => discount.ValidFrom)
                .LessThanOrEqualTo(_ => _.ValidUntil)
                .When(_ => _.ValidFrom != null && _.ValidUntil != null)
                .WithMessage("The valid from property must be a time before valid until");

            RuleFor(discount => discount.ValidUntil)
                .GreaterThanOrEqualTo(_ => _.ValidFrom)
                .When(_ => _.ValidFrom != null && _.ValidUntil != null)
                .WithMessage("The valid until property must be a time after valid from");
        }
    }
}