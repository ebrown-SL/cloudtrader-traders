using CloudTrader.Traders.Api.Models;
using NUnit.Framework;

namespace CloudTrader.Traders.Api.Tests.Models
{
    public class TraderCreationModelTests
    {
        [TestCase(null)]
        public void TraderCreationModel_Id_CantBeNull(int id)
        {
            var trader = new TraderCreationModel
            {
                Id = id
            };

            Assert.IsNotNull(trader.Id);
        }
    }
}
