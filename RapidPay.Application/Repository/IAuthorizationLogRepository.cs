using RapidPay.Application.Repository.Common;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Repository
{
    public interface IAuthorizationLogRepository : IBaseRepository<AuthorizationLog>
    {
        Task<bool> DuplicateAuthorizationExistsAsync(Guid cardId, decimal paymentAmount, DateTime threshold, CancellationToken cancellationToken);
    }
}
