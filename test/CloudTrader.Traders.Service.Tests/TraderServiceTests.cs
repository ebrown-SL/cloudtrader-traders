using AutoMapper;
using CloudTrader.Traders.Models.Api.Request;
using CloudTrader.Traders.Models.Api.Response;
using CloudTrader.Traders.Models.POCO;
using CloudTrader.Traders.Service.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Service.Tests
{
    public class TraderServiceTests
    {
        private readonly TraderProfile profile = new TraderProfile();

        private Tuple<Mock<ITraderRepository>, TraderService> SetupMockConfig()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var mockTraderService = new TraderService(mockTraderRepository.Object, mapper);

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

            var trader = await traderService.CreateTrader(new CreateTraderRequestModel { Balance = 0 });

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
            var isEmpty = traders.Traders.Count == 0;

            Assert.True(isEmpty);
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
            var hasTraders = traders.Traders.Count == 3;

            Assert.True(hasTraders);
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
            var traderMinesExist = traderMinesResponse.CloudStock.Count > 0;

            Assert.True(traderMinesExist);
        }

        [Test]
        public void SetBalance_TraderNotFound_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.SetBalance(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.SetBalance(new Guid(), new SetTraderBalanceRequestModel { Balance = 0 }));
        }

        [Test]
        public async Task SetBalance_TraderExists_ReturnsTraderWithUpdatedBalance()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderGuid = new Guid();
            mockTraderRepository
                .Setup(mock => mock.SetBalance(It.Is<Guid>(i => i == traderGuid), It.Is<int>(bal => bal == 100)))
                .ReturnsAsync(new Trader { Id = traderGuid, Balance = 100 });

            var updatedTraderBalance = await traderService.SetBalance(traderGuid, new SetTraderBalanceRequestModel { Balance = 100 });
            var balanceIsCorrect = updatedTraderBalance.Balance == 100;

            Assert.True(balanceIsCorrect);
        }

        [Test]
        public void UpdateBalance_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.UpdateBalance(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.UpdateBalance(new Guid(), new UpdateTraderBalanceRequestModel() { AmountToAdd = 20 }));
        }

        [Test]
        public async Task UpdateBalance_TraderExists_ReturnsWithUpdatedBalance()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            var traderGuid = new Guid();
            var trader = new Trader() { Id = traderGuid, Balance = 100 };
            var expectedResult = new TraderResponseModel() { Id = traderGuid, Balance = 100 };

            mockTraderRepository
                .Setup(mock => mock.UpdateBalance(It.Is<Guid>(id => id == traderGuid), It.Is<int>(amt => amt == 20)))
                .ReturnsAsync(trader);

            var updatedTrader = await traderService
                .UpdateBalance(traderGuid, new UpdateTraderBalanceRequestModel() { AmountToAdd = 20 });

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
            var traderMineIdIsCorrect = traderMine.MineId == mineId;

            Assert.True(traderMineIdIsCorrect);
        }

        [Test]
        public void SetTraderMine_TraderNotFound_ThrowsTraderNotFoundException()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.SetTraderMine(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync((Trader)null);

            Assert.ThrowsAsync<TraderNotFoundException>
                (
                async () => await traderService
                .SetTraderMine(new Guid(), new SetTraderMineRequestModel
                {
                    MineId = new Guid(),
                    Stock = 1
                })
                );
        }

        [Test]
        public async Task GetTradersByMineId_TradersNotFound_ReturnsEmptyList()
        {
            var (mockTraderRepository, traderService) = SetupMockConfig();

            mockTraderRepository
                .Setup(mock => mock.GetTradersByMineId(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Trader>());

            var traders = await traderService.GetTradersByMineId(new Guid());
            var traderCount = traders.Traders.Count == 0;

            Assert.IsTrue(traderCount);
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
            var resultCount = result.Traders.Count == 2;

            Assert.IsTrue(resultCount);
        }
    }
}