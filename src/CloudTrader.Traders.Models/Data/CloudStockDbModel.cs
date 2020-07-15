using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CloudTrader.Traders.Models.Data
{
    public class CloudStockDbModel
    {
        [Key]
        public int TraderStockId { get; set; }

        [Required]
        public int MineId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [JsonIgnore]
        public TraderDbModel TraderDbModel { get; set; }
        public int TraderDbModelId { get; set; }
    }
}
