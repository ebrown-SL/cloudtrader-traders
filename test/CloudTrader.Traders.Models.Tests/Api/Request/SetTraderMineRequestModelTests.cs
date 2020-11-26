using CloudTrader.Traders.Models.Api.Request;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Tests.Api.Request
{
    public class SetTraderMineRequestModelTests
    {
        private Guid guid = new Guid();

        [TestCase(0, ExpectedResult = true)]
        [TestCase(-1, ExpectedResult = false)]
        [TestCase(100, ExpectedResult = true)]
        [TestCase(-5, ExpectedResult = false)]
        public bool Stock_NegativeValue_IsNotValid_AllOthersValid(int stock)
        {
            var model = new SetTraderMineRequestModel
            {
                MineId = guid,
                Stock = stock
            };

            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, validationContext, result, true);

            return isValid;
        }
    }
}