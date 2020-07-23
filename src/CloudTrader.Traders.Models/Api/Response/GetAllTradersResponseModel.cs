﻿using CloudTrader.Traders.Models.Service;
using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Api
{
    public class GetAllTradersResponseModel
    {
        public List<TraderResponseModel> Traders { get; set; }

        public GetAllTradersResponseModel() { }

        public GetAllTradersResponseModel(List<TraderResponseModel> traders)
        {
            Traders = traders;
        }
    }
}