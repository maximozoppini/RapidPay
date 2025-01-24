using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Common
{
    public class UniversalFeesExchange
    {
        private static UniversalFeesExchange _instance;
        private decimal _lastFeeAmount;
        private DateTime _feeDate;

        private UniversalFeesExchange()
        {
            Random random = new Random();
            _lastFeeAmount = (decimal)(random.NextDouble() * 2);
            _feeDate = DateTime.UtcNow;
        }

        public static UniversalFeesExchange Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UniversalFeesExchange();
                }
                return _instance;
            }
        }

        public decimal GetCurrentFee()
        {
            if ((DateTime.UtcNow - _feeDate).TotalHours >= 1)
            {
                Random random = new Random();
                decimal randomValue = (decimal)(random.NextDouble() * 2);

                _lastFeeAmount *= randomValue;
                _feeDate = DateTime.UtcNow;
            }

            return _lastFeeAmount;
        }
    }
}
