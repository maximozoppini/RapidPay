using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ValidationCode { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
