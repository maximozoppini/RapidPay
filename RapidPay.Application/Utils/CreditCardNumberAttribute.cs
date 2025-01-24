using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Utils
{
    public class CreditCardNumberAttribute : ValidationAttribute
    {
        public CreditCardNumberAttribute()
        {

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string creditCardNumber = value as string;
            if (creditCardNumber == null)
            {
                return new ValidationResult("The credit card is empty");
            }

            creditCardNumber = creditCardNumber.Replace("-", "");
            creditCardNumber = creditCardNumber.Replace(" ", "");

            if (creditCardNumber.Length < 15)
            {
                return new ValidationResult("The credit card must have at least 15 digits"); ;
            }

            int checksum = 0;
            bool evenDigit = false;

            foreach (char digit in creditCardNumber.Reverse())
            {
                if (digit < '0' || digit > '9')
                {
                    return new ValidationResult("The credit card number can not contains other character rather then digits"); ;
                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }

            return (checksum % 10) == 0 ? ValidationResult.Success : new ValidationResult("The credit card number is not valid");
        }
    }
}
