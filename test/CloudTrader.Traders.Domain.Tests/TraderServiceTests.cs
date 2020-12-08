using AutoMapper;
using CloudTrader.Traders.Domain.Models;
using CloudTrader.Traders.Service.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service.Tests
{
    public class TraderServiceTests
    {
        private Tuple<Mock<ITraderRepository>, TraderService> SetupMockConfig()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var mockTraderService = new TraderService(mockTraderRepository.Object);

            return new Tuple<Mock<ITraderRepository>, TraderService>(mockTraderRepository,
                mockTraderService);
        }

        [Test]
        public async Task CreateTrader_ReturnsValidTraderAsync()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.CreateTrader(It.IsAny<Trader>()))
                .ReturnsAsync((Trader trader) => trader);

            var trader = await traderService.CreateTrader(0);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trader, new System.ComponentModel.DataAnnotations.ValidationContext(trader), validationResults, true);

            Assert.True(isValid);
        }

        [TestCase(0)]
        [TestCase(100)]
        [TestCase(1000)]
        public async Task CreateTrader_CreatesTrader_WithExpectedBalance(int initialBalance)
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.CreateTrader(It.Is<Trader>(t => t.Balance == initialBalance)))
                .ReturnsAsync((Trader trader) => trader);

            var trader = await traderService.CreateTrader(initialBalance);

            Assert.AreEqual(initialBalance, trader.Balance);
        }

        [Test]
        public void GetTrader_WithTraderNotFound_ReturnsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTrader(new Guid()));
        }

        [Test]
        public async Task GetTrader_WithTraderFound_ReturnsTrader()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderId = new Guid();
            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync(new Trader { Id = traderId });

            var trader = await traderService.GetTrader(traderId);

            Assert.AreEqual(traderId, trader.Id);
        }

        [Test]
        public async Task GetTraders_NoTraders_ReturnsEmptyList()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTraders())
                .ReturnsAsync(new List<Trader>());

            var traders = await traderService.GetTraders();

            Assert.IsEmpty(traders);
        }

        [Test]
        public async Task GetTraders_TradersExist_ReturnsListOfTraders()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTraders())
                .ReturnsAsync(new List<Trader>() {
                new Trader(), new Trader(), new Trader() });

            var traders = await traderService.GetTraders();

            Assert.AreEqual(3, traders.Count);
        }

        [Test]
        public void GetTraderMines_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTraderMines(new Guid()));
        }

        [Test]
        public async Task GetTraderMines_TraderExistsWithMines_ReturnsTraderMinesResponseWithMines()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync(new Trader
                {
                    CloudStocks = new List<CloudStock>() { new CloudStock() }
                });

            var traderMinesResponse = await traderService.GetTraderMines(new Guid());

            Assert.IsNotEmpty(traderMinesResponse);
        }

        [Test]
        public void SetBalance_TraderNotFound_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.SetBalance(new Guid(), 0));
        }

        [Test]
        public async Task SetBalance_TraderExists_UpdatesBalanceAndReturnsTrader()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var updatedBalance = 200;

            var traderGuid = new Guid();
            var trader = new Trader { Id = traderGuid, Balance = 100 };

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.Is<Guid>(i => i == traderGuid)))
                .ReturnsAsync(trader);

            mockTraderRepository
                .Setup(mock => mock.UpdateTrader(It.Is<Trader>(t => t.Id == traderGuid && t.Balance == updatedBalance)))
                .ReturnsAsync((Trader trader) => trader);

            var updatedTraderBalance = await traderService.SetBalance(traderGuid, updatedBalance);

            Assert.AreEqual(traderGuid, trader.Id);
            Assert.AreEqual(updatedBalance, updatedTraderBalance.Balance);
        }

        [Test]
        public void UpdateBalance_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.UpdateBalance(new Guid(), 20));
        }

        [Test]
        public async Task UpdateBalance_TraderExists_ReturnsWithUpdatedBalance()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderGuid = new Guid();
            var initialBalance = 100;
            var addToBalance = 50;
            var trader = new Trader() { Id = traderGuid, Balance = initialBalance };

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.Is<Guid>(id => id == traderGuid)))
                .ReturnsAsync(trader);

            mockTraderRepository
                .Setup(mock => mock.UpdateTrader(It.Is<Trader>(t => t.Id == traderGuid && t.Balance == 150)))
                .ReturnsAsync(trader);

            var updatedTrader = await traderService.UpdateBalance(traderGuid, addToBalance);

            Assert.AreEqual(initialBalance + addToBalance, updatedTrader.Balance);
        }

        [Test]
        public void GetTraderMine_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTraderMine(new Guid(), new Guid()));
        }

        [Test]
        public void GetTraderMine_MineDoesNotExist_ThrowsMineNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync(new Trader { CloudStocks = new List<CloudStock>() });

            Assert.ThrowsAsync<MineNotFoundException>(async () => await traderService.GetTraderMine(new Guid(), new Guid()));
        }

        [Test]
        public async Task GetTraderMine_MineExists_ReturnsCorrectMineResponseModel()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var mineId = new Guid();
            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync(new Trader
                {
                    CloudStocks = new List<CloudStock>()
                    {
                        new CloudStock { MineId = mineId, Stock = 100 }
                    }
                });

            var traderMine = await traderService.GetTraderMine(new Guid(), mineId);

            Assert.AreEqual(mineId, traderMine.MineId);
        }

        [Test]
        public void SetTraderMine_TraderNotFound_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<Guid>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.SetTraderMine(new Guid(), new Guid(), 1));
        }

        [Test]
        public async Task GetTradersByMineId_TradersNotFound_ReturnsEmptyList()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTradersByMineId(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Trader>());

            var traders = await traderService.GetTradersByMineId(new Guid());

            Assert.IsEmpty(traders);
        }

        [Test]
        public async Task GetTradersByMineId_TradersWithStockFound_ReturnsCorrectResponseModel()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var mineId = new Guid();
            var cloudStocks = new List<CloudStock>() { new CloudStock() { MineId = mineId, Stock = 20 } };
            var trader = new Trader() { Id = new Guid(), Balance = 100, CloudStocks = cloudStocks };
            var traders = new List<Trader>() { trader, trader };

            mockTraderRepository
                .Setup(mock => mock.GetTradersByMineId(It.IsAny<Guid>()))
                .ReturnsAsync(traders);

            var result = await traderService.GetTradersByMineId(mineId);

            Assert.AreEqual(2, result.Count());
        }
    }
}