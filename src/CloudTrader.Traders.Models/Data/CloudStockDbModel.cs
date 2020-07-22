using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CloudTrader.Traders.Models.Data
{
    public class CloudStockDbModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public int MineId { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [JsonIgnore]
        public TraderDbModel TraderDbModel { get; set; }
    }
}
