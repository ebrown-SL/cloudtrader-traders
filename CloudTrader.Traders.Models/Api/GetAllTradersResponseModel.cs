using CloudTrader.Traders.Models.Service;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api
{
    public class GetAllTradersResponseModel
    {
        public List<Trader> Traders { get; set; }

        public GetAllTradersResponseModel() { }

        public GetAllTradersResponseModel(List<Trader> traders)
        {
            Traders = traders;
        }
    }
}
