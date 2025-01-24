using RapidPay.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace RapidPay.Domain.Entities
{
    public class Card : BaseEntity
    {
        [Required]
        public string CardNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string ValidationCode { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

    }
}
