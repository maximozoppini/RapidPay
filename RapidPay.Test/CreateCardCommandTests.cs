using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RapidPay.Application.Common;
using RapidPay.Application.Features.CardFeatures.Commands.CreateCard;
using RapidPay.Application.Mapper;
using RapidPay.Application.Repository;
using RapidPay.Application.Utils;
using RapidPay.Domain.Entities;
using RapidPay.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Test
{
    [TestFixture]
    public class CreateCardCommandTests
    {
        private CreateCardCommandHandler _handler;
        private IMapper _mapper;
        private IEncryptionService _encryptionService;
        private Mock<ICardRepository> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ICardRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _encryptionService = new EncryptionService("TESTSecretKeyForAES256Encryption");

            _handler = new CreateCardCommandHandler(_repositoryMock.Object, _mapper, _encryptionService);
        }

        [Test]
        public async Task Handle_ShouldCreateCardAndReturnDto()
        {
            var command = new CreateCardCommand { CreditLimit = 500, CardNumber = "456745674567456", 
                ExpirationDate = DateTime.Now, ValidationCode = "990" };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Card>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Data.CardNumber);
            Assert.That(result.Data.CreditLimit, Is.EqualTo(500));

            // Verify repository methods were called.
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Card>()), Times.Once);
        }

    }
}
