using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Application.Features.CardFeatures.Commands.CreateCard;
using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.AuthorizeCard
{
    public class AuthorizeCardCommandHandler : IRequestHandler<AuthorizeCardCommand, BaseResult<AuthorizeCardResultDto>>
    {
        private readonly ICardRepository _repository;
        private readonly IAuthorizationLogRepository _authorizationLogRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorizeCardCommandHandler> _logger;

        public AuthorizeCardCommandHandler(ICardRepository repository, IMapper mapper, IEncryptionService encryptionService, 
            IAuthorizationLogRepository authorizationLogRepository, ILogger<AuthorizeCardCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _authorizationLogRepository = authorizationLogRepository;
            _logger = logger;
        }

        public async Task<BaseResult<AuthorizeCardResultDto>> Handle(AuthorizeCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var card = await _repository.GetByCardNumberAsync(_encryptionService.Encrypt(request.CardNumber));

                if (card == null)
                {
                    await LogAuthorization(null, request.PaymentAmount, false, "Card not found.");
                    return new BaseResult<AuthorizeCardResultDto> { Success = false, Message = "Card not found.", Data = new AuthorizeCardResultDto { IsAuthorized = false } };
                }

                if (!card.IsActive)
                {
                    await LogAuthorization(card.Id, request.PaymentAmount, false, "Card is inactive.");
                    return new BaseResult<AuthorizeCardResultDto> { Success = false, Message = "Card is inactive.", Data = new AuthorizeCardResultDto { IsAuthorized = false } };
                }

                var duplicateThreshold = DateTime.UtcNow.AddSeconds(-5);
                bool duplicateExists = await _authorizationLogRepository.DuplicateAuthorizationExistsAsync(card.Id, request.PaymentAmount, duplicateThreshold, cancellationToken);

                if (duplicateExists)
                {
                    await LogAuthorization(card.Id, request.PaymentAmount, false, "Duplicate authorization attempt detected.");
                    return new BaseResult<AuthorizeCardResultDto> { Success = false, Message = "Duplicate authorization attempt detected.", Data = new AuthorizeCardResultDto { IsAuthorized = false } };
                }

                // Mark the card as authorized.
                card.IsAuthorized = true;
                await _repository.UpdateAsync(card);

                await LogAuthorization(card.Id, request.PaymentAmount, true, "Card authorized successfully.");

                return new BaseResult<AuthorizeCardResultDto> { Success = true, Message = "Card authorized successfully.", Data = new AuthorizeCardResultDto { IsAuthorized = true } };
            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                return new BaseResult<AuthorizeCardResultDto> { Success = false, Message = "An error occured while trying to authorize card" };
            }
        }

        private async Task LogAuthorization(Guid? cardId, decimal paymentAmount, bool isAuthorized, string message)
        {
            var log = new AuthorizationLog
            {
                CardId = cardId,
                PaymentAmount = paymentAmount,
                IsAuthorized = isAuthorized,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
            await _authorizationLogRepository.AddAsync(log);
        }
    }
}
