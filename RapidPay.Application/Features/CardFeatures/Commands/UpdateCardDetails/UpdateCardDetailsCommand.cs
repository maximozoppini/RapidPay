using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.UpdateCardDetails
{
    public class UpdateCardDetailsCommand : IRequest<BaseResult<CardDto>>
    {
        public string CardNumber { get; set; }
        public decimal? NewBalance { get; set; }
        public decimal? NewCreditLimit { get; set; }
        public bool? NewStatus { get; set; }
    }
}
