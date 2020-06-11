using System.ComponentModel.DataAnnotations;

namespace CloudtraderTraders.Models
{
    public class TraderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Balance { get; set; }
    }
}
