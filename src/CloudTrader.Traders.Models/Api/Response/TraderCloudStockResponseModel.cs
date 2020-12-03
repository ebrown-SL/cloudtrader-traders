using CloudTrader.Traders.Models.POCO;
using System;
using System.Linq;

namespace CloudTrader.Traders.Models.Api.Response
{
    public class TraderCloudStockResponseModel
    {
        public Guid Id { get; set; }
        public Guid MineId { get; set; }
        public int Stock { get; set; }

        public TraderCloudStockResponseModel()
        {
        }

        public TraderCloudStockResponseModel(Trader trader, Guid mineId)
        {
            var filteredStock = trader.CloudStocks.FirstOrDefault(s => s.MineId == mineId);

            Id = trader.Id;

            MineId = mineId;

            Stock = filteredStock?.Stock ?? 0;
        }
    }
}