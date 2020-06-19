using CloudTrader.Traders.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        public TraderContext(DbContextOptions<TraderContext> options)
            : base(options)
        {
        }

        public DbSet<Trader> Traders { get; set; }
    }
}
