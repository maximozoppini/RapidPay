using Microsoft.EntityFrameworkCore;
using RapidPay.Application.Repository;
using RapidPay.Application.Repository.Common;
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
    public class AuthorizationLogRepository : BaseRepository<AuthorizationLog>, IAuthorizationLogRepository
    {
        public AuthorizationLogRepository(AppDbContext context) : base(context) { }

        public async Task<bool> DuplicateAuthorizationExistsAsync(Guid cardId, decimal paymentAmount, DateTime threshold, CancellationToken cancellationToken)
        {
            return await _context.AuthorizationsLogs.AnyAsync(log =>
                log.CardId == cardId &&
                log.PaymentAmount == paymentAmount &&
                log.Timestamp >= threshold, cancellationToken);
        }

    }
}
