using CloudTrader.Traders.Models.POCO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TraderContext(DbContextOptions<TraderContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Trader> Traders { get; set; }

        public DbSet<CloudStock> CloudStocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseCosmos(
                _configuration["CosmosEndpoint"],
                _configuration["CosmosKey"],
                databaseName: "CloudTrader");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Traders");
            modelBuilder.Entity<Trader>()
                .OwnsMany(t => t.CloudStocks);
        }
    }
}