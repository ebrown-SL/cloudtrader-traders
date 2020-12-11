using CloudTrader.Traders.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        private readonly IConfiguration configuration;

        public TraderContext(DbContextOptions<TraderContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

#nullable disable

        // From MS docs: if the DbSet<TEntity> properties have a public setter, they are automatically initialized when the instance of the derived context is created.
        public DbSet<Trader> Traders { get; set; }

        public DbSet<CloudStock> CloudStocks { get; set; }
#nullable restore

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseCosmos(
                configuration["CosmosEndpoint"],
                configuration["CosmosKey"],
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