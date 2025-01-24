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
        private readonly IMapper _mapper;

        public CreateCardCommandHandler(ICardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResult<CardDto>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var card = new Card
                {
                    CardNumber = request.CardNumber,
                    ValidationCode = request.ValidationCode,
                    ExpirationDate = request.ExpirationDate,
                    UserId = request.UserId
                };

                await _repository.AddAsync(card);

                var cardDto = _mapper.Map<CardDto>(card);
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
