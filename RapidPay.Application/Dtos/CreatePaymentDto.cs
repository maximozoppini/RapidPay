using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Dtos
{
    public class CreatePaymentDto
    {
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}
