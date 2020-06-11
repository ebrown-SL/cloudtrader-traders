using System.Threading.Tasks;
using CloudTrader.Traders.Controllers;
using CloudTrader.Traders.Models;
using CloudTrader.Traders.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CloudTrader.Traders.Tests.Controllers
{
    public class TraderControllerTests
    {
        [Test]
        public async Task GetTrader_TraderNotFound_ReturnsNotFound()
        {
            var mockTraderService = new Mock<ITraderService>();
            var traderController = new TraderController(mockTraderService.Object);

            var result = await traderController.GetTrader(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task CreateTrader_TraderAlreadyExists_ReturnsConflict()
        {
            var mockTraderService = new Mock<ITraderService>();
            mockTraderService.Setup(service => service.GetById(It.IsAny<int>())).ReturnsAsync(new TraderModel());
            var traderController = new TraderController(mockTraderService.Object);

            var result = await traderController.CreateTrader(new TraderModel());

            Assert.IsInstanceOf<ConflictResult>(result);
        }
    }
}
