using System.ComponentModel.DataAnnotations;

namespace CloudTrader.Traders.Models.Api
{
    public class TraderCreationModel
    {
        [Key]
        public int Id { get; set; }
    }
}
