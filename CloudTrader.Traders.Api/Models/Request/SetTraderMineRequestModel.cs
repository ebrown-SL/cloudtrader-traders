using System;
using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Api.Models.Request
{
    public class SetTraderMineRequestModel
    {
        public Guid MineId { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}