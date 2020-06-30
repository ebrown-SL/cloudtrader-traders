using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Api.Models
{
    public class TraderCreationModel
    {
        [Key]
        public int Id { get; set; }
    }
}
