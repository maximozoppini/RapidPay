using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.AuthorizeCard
{
    public class AuthorizeCardCommandValidator : AbstractValidator<AuthorizeCardCommand>
    {
        public AuthorizeCardCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.");
            RuleFor(x => x.PaymentAmount)
                .GreaterThan(0).WithMessage("Payment amount must be positive.");
        }
    }
}
