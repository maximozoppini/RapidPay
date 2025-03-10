using RapidPay.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class AuthorizationLog : BaseEntity
    {
        [ForeignKey("Card")]
        public Guid? CardId { get; set; }
        public Card? Card { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }
        public bool IsAuthorized { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
