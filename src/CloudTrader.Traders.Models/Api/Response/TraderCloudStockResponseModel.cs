using CloudTrader.Traders.Models.POCO;
using System;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api.Response
{
    public class TraderCloudStockResponseModel
    {
        public Guid Id { get; set; }
        public Guid MineId { get; set; }
        public int Stock { get; set; }
    }
}
