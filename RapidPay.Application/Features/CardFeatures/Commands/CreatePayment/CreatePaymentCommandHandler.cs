using AutoMapper;
using MediatR;
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
        private readonly IMapper _mapper;

        public CreatePaymentCommandHandler(ICardRepository cardRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResult<TransactionDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByCardNumberAsync(request.CardNumber);
            if (card == null)
            {
                return new BaseResult<TransactionDto>
                {
                    Success = false,
                    Message = "Card not found."
                };
            }

            var feeExchange = UniversalFeesExchange.Instance;

            var transaction = new Transaction
            {
                CardId = card.Id,
                CardNumber = card.CardNumber,
                Amount = request.Amount,
                Fee = feeExchange.GetCurrentFee(),
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);

            card.Balance = card.Balance + transaction.Amount + (transaction.Amount * feeExchange.GetCurrentFee());
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
