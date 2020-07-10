using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Data
{
    public class TraderDbModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
