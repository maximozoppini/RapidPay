using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RapidPay.Application.Common;
using RapidPay.Application.Repository;
using System.Reflection;

namespace RapidPay.Test;

[TestFixture]
public class UniversalFeeExchangeTests
{
    [TestFixture]
    public class UniversalFeesExchangeTests
    {
        private Mock<IServiceScopeFactory> _scopeFactoryMock;
        private Mock<IServiceScope> _scopeMock;
        private Mock<IPaymentFeeRepository> _feeRepositoryMock;
        private Mock<ILogger<UniversalFeesExchange>> _loggerMock;
        private UniversalFeesExchange _feesExchange;

        [SetUp]
        public void Setup()
        {
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();
            _scopeMock = new Mock<IServiceScope>();
            _feeRepositoryMock = new Mock<IPaymentFeeRepository>();
            _loggerMock = new Mock<ILogger<UniversalFeesExchange>>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IPaymentFeeRepository)))
                .Returns(_feeRepositoryMock.Object);

            _scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(_scopeMock.Object);

            _feesExchange = new UniversalFeesExchange(_scopeFactoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void UpdateFee_Invoked_ChangesCurrentFee()
        {
            // Get the initial fee.
            decimal initialFee = _feesExchange.GetCurrentFee();

            var method = typeof(UniversalFeesExchange)
                .GetMethod("UpdateFee", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, "Could not find UpdateFee method via reflection.");

            method.Invoke(_feesExchange, new object[] { null });

            decimal updatedFee = _feesExchange.GetCurrentFee();
            Assert.That(updatedFee, Is.Not.EqualTo(initialFee));
        }

        [TearDown]
        public void TearDown()
        {
            _feesExchange?.Dispose();
        }
    }
}
