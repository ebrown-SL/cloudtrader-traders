using CloudTrader.Traders.Models.POCO;
using System;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api.Response
{
    public class TraderCloudStockResponseModel
    {
        public Guid Id { get; set; }
        public int Balance { get; set; }
        public List<CloudStock> CloudStock { get; set; }
    }
}
