using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<BaseResult<TransactionDto>>
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
