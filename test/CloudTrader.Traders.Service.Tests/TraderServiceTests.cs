using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
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

            mockTraderRepository.Setup(mock => mock.SaveTrader(It.IsAny<TraderDbModel>())).ReturnsAsync(new TraderDbModel { Id = 1, Balance = 0 });

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

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync((TraderDbModel) null);

            Assert.ThrowsAsync<TraderNotFoundException>(async () => await traderService.GetTrader(1));
        }

        [Test]
        public async Task GetTrader_WithTraderFound_ReturnsTrader()
        {
            var mockTraderRepository = new Mock<ITraderRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(config);
            var traderService = new TraderService(mockTraderRepository.Object, mapper);

            mockTraderRepository.Setup(mock => mock.GetTrader(It.IsAny<int>())).ReturnsAsync(new TraderDbModel { Id = 1 });

            var trader = await traderService.GetTrader(1);

            Assert.AreEqual(1, trader.Id);
        }
    }
}
