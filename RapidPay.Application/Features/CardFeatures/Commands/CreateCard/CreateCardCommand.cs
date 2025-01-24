using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.CreateCard
{
    public class CreateCardCommand : IRequest<BaseResult<CardDto>>
    {
        public string CardNumber { get; set; }
        public string ValidationCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid UserId { get; set; }
    }
}
