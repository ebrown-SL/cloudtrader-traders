using System;
using System.Linq;

namespace CloudTrader.Traders.Domain.Models
{
    public class TraderCloudStock
    {
        public Guid TraderId { get; set; }
        public Guid MineId { get; set; }
        public int Stock { get; set; }

        public TraderCloudStock()
        {
        }

        public TraderCloudStock(Trader trader, Guid mineId)
        {
            var filteredStock = trader.CloudStocks.FirstOrDefault(s => s.MineId == mineId);

            TraderId = trader.Id;

            MineId = mineId;

            Stock = filteredStock?.Stock ?? 0;
        }
    }
}