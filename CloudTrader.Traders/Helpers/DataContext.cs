using CloudTrader.Traders.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Trader> Traders { get; set; }
    }
}
