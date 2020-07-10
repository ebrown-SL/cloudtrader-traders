using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Service
{
    public class Trader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
