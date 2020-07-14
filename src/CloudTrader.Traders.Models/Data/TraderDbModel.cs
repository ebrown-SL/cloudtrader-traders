using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Data
{
    public class TraderDbModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }

        public virtual List<CloudStockDbModel> CloudStock { get; set; }
    }
}
