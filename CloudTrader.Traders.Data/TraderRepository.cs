using CloudTrader.Traders.Domain.Models;
using CloudTrader.Traders.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudTrader.Traders.Data
{
    public class TraderRepository : ITraderRepository
    {
        private readonly TraderContext context;

        public TraderRepository(TraderContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
        }

        public async Task<Trader> GetTrader(Guid id)
        {
            var trader = await context
                .Traders
                .FirstOrDefaultAsync(t => t.Id == id);

            return trader;
        }

        public async Task<List<Trader>> GetTraders()
        {
            var traders = await context.Traders.ToListAsync();
            return traders;
        }

        public async Task<List<Trader>> GetTradersByMineId(Guid mineId)
        {
            var traders = await context.Traders
                .ToListAsync();

            var filteredTraders = traders
                .Where(t => t.CloudStocks.Any(s => s.MineId == mineId))
                .ToList();

            return filteredTraders;
        }

        public async Task<Trader> CreateTrader(Trader trader)
        {
            trader.Id = new Guid();
            context.Traders.Add(trader);
            await context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> UpdateTrader(Trader trader)
        {
            await context.SaveChangesAsync();
            return trader;
        }
    }
}