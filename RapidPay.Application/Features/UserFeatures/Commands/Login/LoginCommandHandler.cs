using AutoMapper;
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

namespace RapidPay.Application.Features.UserFeatures.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResult<UserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.LoginDto.Username);
            if (user == null || !Extensions.VerifyPassword(request.LoginDto.Password, user.Password))
            {
                return new BaseResult<UserDto>
                {
                    Success = false,
                    Message = "Invalid username or password."
                };
            }

            var userDto = _mapper.Map<UserDto>(user);
            return new BaseResult<UserDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = userDto
            };
        }
    }
}
