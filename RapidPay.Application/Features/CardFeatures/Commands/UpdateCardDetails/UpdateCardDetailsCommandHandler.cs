using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Application.Features.CardFeatures.Commands.CreatePayment;
using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.UpdateCardDetails
{
    public class UpdateCardDetailsCommandHandler : IRequestHandler<UpdateCardDetailsCommand, BaseResult<CardDto>>
    {
        private readonly ILogger<UpdateCardDetailsCommandHandler> _logger;
        private readonly ICardRepository _cardRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ICardChangeLogRepository _cardChangeLogRepository;
        private readonly IMapper _mapper;

        public UpdateCardDetailsCommandHandler(ICardRepository cardRepository, ILogger<UpdateCardDetailsCommandHandler> logger, IEncryptionService encryptionService,
            ICardChangeLogRepository cardChangeLogRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _logger = logger;
            _encryptionService = encryptionService;
            _cardChangeLogRepository = cardChangeLogRepository;
            _mapper = mapper;
        }

        public async Task<BaseResult<CardDto>> Handle(UpdateCardDetailsCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByCardNumberAsync(_encryptionService.Encrypt(request.CardNumber));

            if (card == null)
            {
                _logger.LogTrace("UpdateCardDetails-error-CardNotFound");
                return new BaseResult<CardDto>
                {
                    Success = false,
                    Message = "Card not found."
                };
            }

            await _cardChangeLogRepository.AddAsync(new CardChangeLog
            {
                CardId = card.Id,
                Changes = JsonConvert.SerializeObject(new
                {
                    Old = new { card.Id, card.Balance, card.CreditLimit, card.IsActive },
                    New = new { request.NewBalance, request.NewCreditLimit, request.NewStatus }
                })
            });

            if (request.NewBalance.HasValue)
            {
                card.Balance = request.NewBalance.Value;
            }
            if (request.NewCreditLimit.HasValue)
            {
                card.CreditLimit = request.NewCreditLimit.Value;
            }
            if (request.NewStatus.HasValue)
            {
                card.IsActive = request.NewStatus.Value;
            }

            await _cardRepository.UpdateAsync(card);

            return new BaseResult<CardDto> { Success = true, Message = "Card details updated successfully.", Data = _mapper.Map<CardDto>(card) };
        
        }
    }
}
