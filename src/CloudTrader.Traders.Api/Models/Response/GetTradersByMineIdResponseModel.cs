using System.Collections.Generic;

namespace CloudTrader.Traders.Api.Models.Response
{
    public class GetTradersByMineIdResponseModel
    {
        public List<TraderCloudStockResponseModel> Traders { get; set; }

        public GetTradersByMineIdResponseModel()
        {
        }

        public GetTradersByMineIdResponseModel(List<TraderCloudStockResponseModel> traders)
        {
            Traders = traders;
        }
    }
}