using RapidPay.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class CardChangeLog : BaseEntity
    {
        [ForeignKey("Card")]
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        public string Changes { get; set; }
    }
}
