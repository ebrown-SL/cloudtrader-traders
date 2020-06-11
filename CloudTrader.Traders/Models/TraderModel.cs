using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models
{
    public class TraderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
