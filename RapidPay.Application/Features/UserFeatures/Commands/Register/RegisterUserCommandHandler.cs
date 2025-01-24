using AutoMapper;
using MediatR;
using Org.BouncyCastle.Crypto.Generators;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using RapidPay.Application.Repository;
using RapidPay.Application.Utils;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.UserFeatures.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResult<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userExists = await _userRepository.GetByUsernameAsync(request.UserDto.Username);
                if (userExists != null)
                {
                    return new BaseResult<UserDto>
                    {
                        Success = false,
                        Message = "User already exist.",
                        Data = request.UserDto
                    };
                }

                var user = _mapper.Map<User>(request.UserDto);
                user.Password = request.UserDto.Password.HashPassword();

                await _userRepository.AddAsync(user);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Password = request.UserDto.Password;

                return new BaseResult<UserDto>
                {
                    Success = true,
                    Message = "User registered successfully.",
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<UserDto>
                {
                    Success = false,
                    Message = "Failed to register user.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
