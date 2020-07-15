using CloudTrader.Traders.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        public TraderContext(DbContextOptions<TraderContext> options) : base(options) { }

        public DbSet<TraderDbModel> Traders { get; set; }

        public DbSet<CloudStockDbModel> CloudStocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseInMemoryDatabase(databaseName: "Traders")
                .EnableSensitiveDataLogging();
        }
    }
}
