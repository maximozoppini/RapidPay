using MediatR;
using RapidPay.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Querys
{
    public class GetCardBalanceQuery : IRequest<BaseResult<decimal>>
    {
        public string CardNumber { get; set; }
    }
}
