using System;
using System.Collections.Generic;

namespace CloudTrader.Traders.Domain.Models
{
    public class Trader
    {
        public Guid Id { get; set; }

        public int Balance { get; set; }

        public ICollection<CloudStock> CloudStocks { get; set; }

        public Trader()
        {
            CloudStocks = new HashSet<CloudStock>();
        }
    }
}