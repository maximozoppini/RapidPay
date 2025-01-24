using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Querys
{
    public class GetCardBalanceQueryHandler : IRequestHandler<GetCardBalanceQuery, BaseResult<decimal>>
    {
        private readonly ICardRepository _cardRepository;

        public GetCardBalanceQueryHandler(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<BaseResult<decimal>> Handle(GetCardBalanceQuery request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByCardNumberAsync(request.CardNumber);
            if (card == null)
            {
                return new BaseResult<decimal>
                {
                    Success = false,
                    Message = "Card not found."
                };
            }

            return new BaseResult<decimal>
            {
                Success = true,
                Message = "Card balance retrieved successfully.",
                Data = card.Balance
            };
        }
    }
}
