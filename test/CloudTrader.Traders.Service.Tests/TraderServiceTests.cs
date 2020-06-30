using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudTrader.Traders.Service.Exceptions;
using Moq;
using NUnit.Framework;

namespace CloudTrader.Traders.Service.Tests
{
    public class TraderServiceTests
    {
        [Test]
        public void CreateTrader_WithTraderIdAlreadyExists_ThrowsTraderAlreadyExistsException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var traderService = new TraderService(mockTraderRepository.Object);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync(new Trader());

            Assert.ThrowsAsync<TraderAlreadyExistsException>(async () => await traderService.CreateTrader(1));
        }

        [Test]
        public async Task CreateTrader_WithValidId_ReturnsValidTraderAsync()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var traderService = new TraderService(mockTraderRepository.Object);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync((Trader) null);

            var trader = await traderService.CreateTrader(1);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trader, new ValidationContext(trader), validationResults, true);

            Assert.True(isValid);
        }

        [Test]
        public async Task CreateTrader_WithValidId_ReturnsCorrectTraderId()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var traderService = new TraderService(mockTraderRepository.Object);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync((Trader)null);

            var trader = await traderService.CreateTrader(1);

            Assert.AreEqual(1, trader.Id);
        }

        [Test]
        public void GetTrader_WithTraderNotFound_ReturnsTraderNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var traderService = new TraderService(mockTraderRepository.Object);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync((Trader) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTrader(1));
        }

        [Test]
        public async Task GetTrader_WithTraderFound_ReturnsTrader()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var traderService = new TraderService(mockTraderRepository.Object);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync(new Trader { Id = 1 });

            var trader = await traderService.GetTrader(1);

            Assert.AreEqual(1, trader.Id);
        }
    }
}
