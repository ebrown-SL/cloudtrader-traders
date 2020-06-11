using NUnit.Framework;
using CloudTrader.Traders.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Tests
{
    public class TraderModelTests
    {
        [TestCase(null)]
        public void Id_CantBeNull(int id)
        {
            var trader = new TraderModel
            {
                Id = id,
                Balance = 100
            };

            Assert.IsNotNull(trader.Id);
        }

        [TestCase(null)]
        public void Balance_CantBeNull(int balance)
        {
            var trader = new TraderModel
            {
                Id = 1,
                Balance = balance
            };

            Assert.IsNotNull(trader.Balance);
        }

        [Test]
        public void Balance_Zero_IsValid()
        {
            var trader = new TraderModel
            {
                Id = 1,
                Balance = 0
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trader, new ValidationContext(trader), validationResults);

            Assert.True(isValid);
        }
    }
}
