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

namespace RapidPay.Application.Features.CardFeatures.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, BaseResult<CardDto>>
    {
        private readonly ICardRepository _repository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private static readonly Random _random = new Random();

        public CreateCardCommandHandler(ICardRepository repository, IMapper mapper, IEncryptionService encryptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        public async Task<BaseResult<CardDto>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var card = new Card
                {
                    CardNumber = _encryptionService.Encrypt(request.CardNumber),
                    ValidationCode = request.ValidationCode,
                    ExpirationDate = request.ExpirationDate,
                    CreditLimit = request.CreditLimit,
                    UserId = request.UserId,
                    Balance = (decimal)_random.NextDouble() * (10000 - 100) + 100,
                    IsActive = true,
                    IsAuthorized = false
                };

                await _repository.AddAsync(card);

                var cardDto = _mapper.Map<CardDto>(card);
                cardDto.CardNumber = request.CardNumber;
                return new BaseResult<CardDto>
                {
                    Success = true,
                    Message = "Card created successfully.",
                    Data = cardDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<CardDto>
                {
                    Success = false,
                    Message = "Failed to create card.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
