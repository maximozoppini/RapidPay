using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Features.UserFeatures.Commands.Register
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.UserDto.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must be less than 50 characters.");

            RuleFor(x => x.UserDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.UserDto.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(20).WithMessage("Role must be less than 20 characters.")
                .Must(x => new[] { "admin", "user" }.Contains(x)).WithMessage("values for role must be either admin or user");
        }
    }
}
