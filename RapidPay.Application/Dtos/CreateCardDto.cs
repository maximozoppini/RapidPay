using RapidPay.Application.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class CreateCardDto
    {
        [Required]
        [CreditCardNumber]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression("(^[0-9]{3,4}$)", ErrorMessage = "Please enter a valid card code")]
        public string ValidationCode { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
