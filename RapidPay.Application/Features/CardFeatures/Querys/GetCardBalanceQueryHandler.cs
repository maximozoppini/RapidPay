using FluentValidation;
using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Application.Repository;
using RapidPay.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RapidPay.Application.Features.CardFeatures.Querys
{
    public class GetCardBalanceQueryHandler : IRequestHandler<GetCardBalanceQuery, BaseResult<GetCardBalanceDto>>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IValidator<GetCardBalanceQuery> _validator;
        private readonly IEncryptionService _encryptionService;

        public GetCardBalanceQueryHandler(ICardRepository cardRepository, IValidator<GetCardBalanceQuery> validator, IEncryptionService encryptionService)
        {
            _cardRepository = cardRepository;
            _validator = validator;
            _encryptionService = encryptionService;
        }

        public async Task<BaseResult<GetCardBalanceDto>> Handle(GetCardBalanceQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var card = await _cardRepository.GetByCardNumberAsync(_encryptionService.Encrypt(request.CardNumber));
            if (card == null)
            {
                return new BaseResult<GetCardBalanceDto>
                {
                    Success = false,
                    Message = "Card not found."
                };
            }

            return new BaseResult<GetCardBalanceDto>
            {
                Success = true,
                Message = "Card balance retrieved successfully.",
                Data = new GetCardBalanceDto { Balance = card.Balance, CreditLimit = card.CreditLimit }
            };
        }
    }
}
