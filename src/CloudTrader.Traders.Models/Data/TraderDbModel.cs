using System.Collections.Generic;

namespace CloudTrader.Traders.Models.Data
{
    public class TraderDbModel
    {
        public int Id { get; set; }

        public int Balance { get; set; }

        public virtual List<CloudStockDbModel> CloudStockDbModels { get; set; }
    }
}
