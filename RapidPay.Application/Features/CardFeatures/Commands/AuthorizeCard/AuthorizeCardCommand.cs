using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.AuthorizeCard
{
    public class AuthorizeCardCommand : IRequest<BaseResult<AuthorizeCardResultDto>>
    {
        public string CardNumber { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
