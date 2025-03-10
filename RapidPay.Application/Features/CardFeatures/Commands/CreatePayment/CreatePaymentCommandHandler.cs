using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.CardFeatures.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, BaseResult<TransactionDto>>
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePaymentCommandHandler> _logger;
        private readonly IUniversalFeesExchange _feesService;

        public CreatePaymentCommandHandler(ICardRepository cardRepository, ITransactionRepository transactionRepository, 
            IMapper mapper, IEncryptionService encryptionService, ILogger<CreatePaymentCommandHandler> logger, IUniversalFeesExchange feesService)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _logger = logger;
            _feesService = feesService;
        }

        public async Task<BaseResult<TransactionDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByCardNumberAsync(_encryptionService.Encrypt(request.CardNumber));

            if (card == null)
            {
                _logger.LogTrace("CreatePayment-error-CardNotFound");
                return new BaseResult<TransactionDto>
                {
                    Success = false,
                    Message = "Card not found."
                };
            }

            if (!card.IsActive || !card.IsAuthorized)
            {
                _logger.LogTrace("CreatePayment-error-CardNotActiveAuthorized");
                return new BaseResult<TransactionDto>
                {
                    Success = false,
                    Message = "Card not active or not authorized."
                };
            }

            // Retrieve current fee from the UFE service.
            decimal currentFee = _feesService.GetCurrentFee();
            decimal totalDeduction = request.Amount + currentFee;

            if (card.Balance < totalDeduction)
            {
                _logger.LogTrace("CreatePayment-error-InsufficientFunds");
                return new BaseResult<TransactionDto>
                {
                    Success = false,
                    Message = "Insufficient balance to cover payment and fees."
                };
            }

            var transaction = new Transaction
            {
                CardId = card.Id,
                CardNumber = card.CardNumber,
                Amount = request.Amount,
                Fee = currentFee,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);

            card.Balance -= totalDeduction;
            await _cardRepository.UpdateAsync(card);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            return new BaseResult<TransactionDto>
            {
                Success = true,
                Message = "Payment created successfully.",
                Data = transactionDto
            };
        }
    }
}
