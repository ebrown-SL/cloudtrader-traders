using CloudTrader.Traders.Models.POCO;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api.Response
{
    public class GetTraderMinesResponseModel
    {
        public List<CloudStock> CloudStock { get; set; }
    }
}