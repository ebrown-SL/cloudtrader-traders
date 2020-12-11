using CloudTrader.Traders.Domain.Models;
using System.Collections.Generic;

namespace CloudTrader.Traders.Api.Models.Response
{
    public class GetTraderMinesResponseModel
    {
        public List<CloudStock> CloudStock { get; set; }
    }
}