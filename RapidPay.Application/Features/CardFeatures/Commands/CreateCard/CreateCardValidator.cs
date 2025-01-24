using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.CreateCard
{
    public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(16).WithMessage("Card number must be 16 characters long.");

            RuleFor(x => x.ValidationCode)
                .NotEmpty().WithMessage("Validation code is required.")
                .Length(3, 4).WithMessage("Validation code must be 3 or 4 characters long.");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.");
        }
    }
}
