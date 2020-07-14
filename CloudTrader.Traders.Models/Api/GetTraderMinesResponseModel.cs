using CloudTrader.Traders.Models.Service;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api
{
    public class GetTraderMinesResponseModel
    {
        public int TraderId { get; set; }

        public List<CloudStockDetail> CloudStock { get; set; }
    }
}
