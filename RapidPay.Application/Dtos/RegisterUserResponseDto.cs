using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class RegisterUserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
