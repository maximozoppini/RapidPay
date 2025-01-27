using MediatR;
using RapidPay.Application.Common;
using RapidPay.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.UserFeatures.Commands.Register
{
    public class RegisterUserCommand : IRequest<BaseResult<RegisterUserResponseDto>>
    {
        public UserDto UserDto { get; set; }

        public RegisterUserCommand(UserDto userDto)
        {
            UserDto = userDto;
        }
    }
}
