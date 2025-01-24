using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
