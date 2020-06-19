using CloudTrader.Traders.Service;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        public TraderContext(DbContextOptions<TraderContext> options)
            : base(options)
        {
        }

        public DbSet<TraderDbModel> Traders { get; set; }
    }
}
