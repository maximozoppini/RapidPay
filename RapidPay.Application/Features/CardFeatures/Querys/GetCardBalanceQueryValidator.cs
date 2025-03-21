﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Querys
{
    public class GetCardBalanceQueryValidator : AbstractValidator<GetCardBalanceQuery>
    {
        public GetCardBalanceQueryValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(15).WithMessage("Card number must be 15 characters long.");
        }
    }
}
