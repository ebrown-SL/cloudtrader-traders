using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CloudTrader.Traders.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace CloudTrader.Traders.Data
{
    public class TraderContext : DbContext
    {
        public TraderContext(DbContextOptions<TraderContext> options) : base(options) { }

        public DbSet<TraderDbModel> Traders { get; set; }

        public DbSet<CloudStockDbModel> CloudStocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry = {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri("https://cloudtradervault.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secretEndpoint = client.GetSecret("CosmosEndpoint");
            KeyVaultSecret secretKey = client.GetSecret("CosmosKey");

            optionsBuilder
                .UseCosmos(
                secretEndpoint.Value,
                secretKey.Value,
                databaseName: "CloudTrader");
        }
    }
}
