using RapidPay.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string CardNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; }

        public DateTime TransactionDate { get; set; }

        [ForeignKey("Card")]
        public Guid CardId { get; set; }
        public Card Card { get; set; }
    }
}
