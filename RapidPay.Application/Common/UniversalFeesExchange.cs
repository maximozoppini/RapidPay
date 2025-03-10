using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Common
{
    public class UniversalFeesExchange : IUniversalFeesExchange, IHostedService, IDisposable
    {
        //private readonly IPaymentFeeRepository _paymentFeeRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UniversalFeesExchange> _logger;
        private Timer _timer;
        private decimal _currentFee;
        private readonly Random _random = new Random();

        public UniversalFeesExchange(IServiceScopeFactory scopeFactory, ILogger<UniversalFeesExchange> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            // Starting fee
            _currentFee = (decimal)(_random.NextDouble() * 2);
        }

        public decimal GetCurrentFee() => _currentFee;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async void UpdateFee(object state)
        {
            try
            {
                _logger.LogInformation("New fee value");

                decimal multiplier = (decimal)_random.NextDouble() * 2;
                _currentFee = _currentFee * multiplier;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var feeRepository = scope.ServiceProvider.GetRequiredService<IPaymentFeeRepository>();
                    var feeHistory = new PaymentFee
                    {
                        Fee = _currentFee
                    };

                    await feeRepository.AddAsync(feeHistory);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving fee history.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    
    
        //private static UniversalFeesExchange _instance;
        //private decimal _lastFeeAmount;
        //private DateTime _feeDate;

        //private UniversalFeesExchange()
        //{
        //    Random random = new Random();
        //    _lastFeeAmount = (decimal)(random.NextDouble() * 2);
        //    _feeDate = DateTime.UtcNow;
        //}

        //public static UniversalFeesExchange Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new UniversalFeesExchange();
        //        }
        //        return _instance;
        //    }
        //}

        //public decimal GetCurrentFee()
        //{
        //    if ((DateTime.UtcNow - _feeDate).TotalHours >= 1)
        //    {
        //        Random random = new Random();
        //        decimal randomValue = (decimal)(random.NextDouble() * 2);

        //        _lastFeeAmount *= randomValue;
        //        _feeDate = DateTime.UtcNow;
        //    }

        //    return _lastFeeAmount;
        //}
    }
}
