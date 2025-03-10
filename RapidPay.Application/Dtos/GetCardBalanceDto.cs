using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class GetCardBalanceDto
    {
        public decimal Balance { get; set; }
        public decimal? CreditLimit { get; set; }
    }
}
