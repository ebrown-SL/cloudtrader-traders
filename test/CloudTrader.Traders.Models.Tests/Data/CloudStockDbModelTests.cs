using CloudTrader.Traders.Models.Data;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Tests.Data
{
    public class CloudStockDbModelTests
    {
        [TestCase(-1)]
        public void Stock_Negative_IsInvalid(int stock)
        {
            var cloudStockDbModel = new CloudStockDbModel
            {
                Id = 1,
                MineId = 1,
                Stock = stock,
                TraderDbModel = new TraderDbModel()
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cloudStockDbModel, new ValidationContext(cloudStockDbModel), validationResults, true);

            Assert.False(isValid);
        }

        [TestCase(0)]
        public void Stock_Zero_IsValid(int stock)
        {
            var cloudStockDbModel = new CloudStockDbModel
            {
                Id = 1,
                MineId = 1,
                Stock = stock,
                TraderDbModel = new TraderDbModel()
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cloudStockDbModel, new ValidationContext(cloudStockDbModel), validationResults, true);

            Assert.True(isValid);
        }

        [TestCase(1)]
        public void Stock_Positive_IsValid(int stock)
        {
            var cloudStockDbModel = new CloudStockDbModel
            {
                Id = 1,
                MineId = 1,
                Stock = stock,
                TraderDbModel = new TraderDbModel()
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cloudStockDbModel, new ValidationContext(cloudStockDbModel), validationResults, true);

            Assert.True(isValid);
        }
    }
}
