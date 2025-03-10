using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using RapidPay.Entities.Context;
using RapidPay.Persistance.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Persistance.Repository
{
    public class PaymentFeeRepository : BaseRepository<PaymentFee>, IPaymentFeeRepository
    {
        public PaymentFeeRepository(AppDbContext context) : base(context) { }

    }
}
