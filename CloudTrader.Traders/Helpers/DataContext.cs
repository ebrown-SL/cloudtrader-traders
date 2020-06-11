using CloudTrader.Traders.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<TraderModel> Traders { get; set; }
    }
}
