using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.UpdateCardDetails
{
    public class UpdateCardDetailsCommandValidator : AbstractValidator<UpdateCardDetailsCommand>
    {
        public UpdateCardDetailsCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(15).WithMessage("Card number must be 15 characters long.");
            RuleFor(x => x)
                .Must(x => x.NewBalance.HasValue || x.NewCreditLimit.HasValue || x.NewStatus.HasValue)
                .WithMessage("At least one field must be updated.");
        }
    }
}
