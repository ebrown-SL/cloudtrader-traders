using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using CloudTrader.Traders.Models.Api;
using CloudTrader.Traders.Models.Data;
using CloudTrader.Traders.Service.Exceptions;
using Moq;
using NUnit.Framework;

namespace CloudTrader.Traders.Service.Tests
{
    public class TraderServiceTests
    {
        private readonly TraderProfile profile = new TraderProfile();

        [Test]
        public async Task CreateTrader_ReturnsValidTraderAsync()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.SaveTrader(It.IsAny<TraderDbModel>()))
                .ReturnsAsync(new TraderDbModel { Id = 1, Balance = 0 });

            var trader = await traderService.CreateTrader();

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trader, new System.ComponentModel.DataAnnotations.ValidationContext(trader), validationResults, true);

            Assert.True(isValid);
        }

        [Test]
        public void GetTrader_WithTraderNotFound_ReturnsTraderNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync((TraderDbModel) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTrader(1));
        }

        [Test]
        public async Task GetTrader_WithTraderFound_ReturnsTrader()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync(new TraderDbModel { Id = 1 });

            var trader = await traderService.GetTrader(1);

            Assert.AreEqual(1, trader.Id);
        }

        [Test]
        public async Task GetTraders_NoTraders_ReturnsEmptyList()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTraders())
                .ReturnsAsync(new List<TraderDbModel>());

            var traders = await traderService.GetTraders();
            var isEmpty = traders.Traders.Count == 0;

            Assert.True(isEmpty);
        }

        [Test]
        public async Task GetTraders_TradersExist_ReturnsListOfTraders()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTraders())
                .ReturnsAsync(new List<TraderDbModel>() {
                new TraderDbModel(), new TraderDbModel(), new TraderDbModel() });

            var traders = await traderService.GetTraders();
            var hasTraders = traders.Traders.Count == 3;

            Assert.True(hasTraders);
        }

        [Test]
        public void GetTraderMines_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync((TraderDbModel) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTraderMines(1));
        }

        [Test]
        public async Task GetTraderMines_TraderExistsWithMines_ReturnsTraderMinesResponseWithMines()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync(new TraderDbModel { 
                    CloudStockDbModels = new List<CloudStockDbModel>() { new CloudStockDbModel() } });

            var traderMinesResponse = await traderService.GetTraderMines(1);
            var traderMinesExist = traderMinesResponse.CloudStock.Count > 0;

            Assert.True(traderMinesExist);
        }

        [Test]
        public void SetBalance_TraderNotFound_ThrowsTraderNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.SetBalance(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((TraderDbModel) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.SetBalance(1, new SetTraderBalanceRequestModel { Balance = 0 }));
        }

        [Test]
        public async Task SetBalance_TraderExists_ReturnsTraderWithUpdatedBalance()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.SetBalance(It.Is<int>(i => i == 1), It.Is<int>(bal => bal == 100)))
                .ReturnsAsync(new TraderDbModel { Id = 1, Balance = 100 });

            var updatedTraderBalance = await traderService.SetBalance(1, new SetTraderBalanceRequestModel { Balance = 100 });
            var balanceIsCorrect = updatedTraderBalance.Balance == 100;

            Assert.True(balanceIsCorrect);
        }

        [Test]
        public void GetTraderMine_TraderDoesNotExist_ThrowsTraderNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync((TraderDbModel) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTraderMine(1, 1));
        }

        [Test]
        public void GetTraderMine_MineDoesNotExist_ThrowsMineNotFoundException()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync(new TraderDbModel { CloudStockDbModels = new List<CloudStockDbModel>() });

            Assert.ThrowsAsync<MineNotFoundException>(async () => await traderService.GetTraderMine(1, 1));
        }

        [Test]
        public async Task GetTraderMine_MineExists_ReturnsCorrectMineResponseModel()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository
                .Setup(mock => mock.GetTrader(It.IsAny<int>()))
                .ReturnsAsync(new TraderDbModel
                {
                    CloudStockDbModels = new List<CloudStockDbModel>()
                    {
                        new CloudStockDbModel { Id = 1, MineId = 1, Stock = 100 }
                    }
                });

            var traderMine = await traderService.GetTraderMine(1, 1);
            var traderMineIdIsCorrect = traderMine.MineId == 1;

            Assert.True(traderMineIdIsCorrect);
        }
    }
}
