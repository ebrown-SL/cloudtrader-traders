using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Data
{
    public class TraderDbModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
