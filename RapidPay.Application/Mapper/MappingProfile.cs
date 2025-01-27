using AutoMapper;
using RapidPay.Application.Dtos;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Card, CardDto>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterUserResponseDto>();
        }
    }
}
