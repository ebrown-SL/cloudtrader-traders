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
                .Setup(mock => mock.SaveTrader(It.IsAny<Trader>()))
                .ReturnsAsync(new Trader { Id = new Guid(), Balance = 0 });

            var trader = await traderService.CreateTrader(0);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trader, new System.ComponentModel.DataAnnotations.ValidationContext(trader), validationResults, true);

            Assert.True(isValid);
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
                .Setup(mock => mock.SetBalance(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.SetBalance(new Guid(), 0));
        }

        [Test]
        public async Task SetBalance_TraderExists_ReturnsTraderWithUpdatedBalance()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderGuid = new Guid();
            mockTraderRepository
                .Setup(mock => mock.SetBalance(It.Is<Guid>(i => i == traderGuid), It.Is<int>(bal => bal == 100)))
                .ReturnsAsync(new Trader { Id = traderGuid, Balance = 100 });

            var updatedTraderBalance = await traderService.SetBalance(traderGuid, 100);

            Assert.AreEqual(100, updatedTraderBalance.Balance);
        }

        [Test]
        public void UpdateBalance_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.UpdateBalance(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.UpdateBalance(new Guid(), 20));
        }

        [Test]
        public async Task UpdateBalance_TraderExists_ReturnsWithUpdatedBalance()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderGuid = new Guid();
            var trader = new Trader() { Id = traderGuid, Balance = 100 };
            var expectedResult = new Trader() { Id = traderGuid, Balance = 100 };

            mockTraderRepository
                .Setup(mock => mock.UpdateBalance(It.Is<Guid>(id => id == traderGuid), It.Is<int>(amt => amt == 20)))
                .ReturnsAsync(trader);

            var updatedTrader = await traderService
                .UpdateBalance(traderGuid, 20);

            Assert.AreEqual(expectedResult.Balance, updatedTrader.Balance);
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
                .Setup(mock => mock.SetTraderMine(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
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