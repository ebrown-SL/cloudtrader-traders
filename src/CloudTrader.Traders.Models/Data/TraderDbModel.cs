using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudTrader.Traders.Models.Data
{
    public class TraderDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public int Balance { get; set; }

        public virtual List<CloudStockDbModel> CloudStockDbModels { get; set; }
    }
}
