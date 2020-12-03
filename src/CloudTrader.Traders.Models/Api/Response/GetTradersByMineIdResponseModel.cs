﻿using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api.Response
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