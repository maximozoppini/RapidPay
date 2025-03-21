﻿using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Querys
{
    public class GetCardBalanceQuery : IRequest<BaseResult<GetCardBalanceDto>>
    {
        public string CardNumber { get; set; }
    }
}
