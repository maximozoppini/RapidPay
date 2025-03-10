using RapidPay.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class PaymentFee : BaseEntity
    {
        public decimal Fee { get; set; }
    }
}
