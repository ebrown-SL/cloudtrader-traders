using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudTrader.Traders.Models.POCO;
using CloudTrader.Traders.Service;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public class TraderRepository : ITraderRepository
    {
        private readonly TraderContext _context;

        public TraderRepository(TraderContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<Trader> SetTraderMine(Guid id, Guid mineId, int stock)
        {
            var trader = await GetTrader(id);
            if (trader == null)
            {
                return trader;
            }
            trader.CloudStocks ??= new HashSet<CloudStock>();
            var existingMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (existingMine == null)
            {
                trader.CloudStocks.Add(new CloudStock { MineId = mineId, Stock = stock }) ;
            } 
            else
            {
                existingMine.Stock = stock;
            }
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> GetTrader(Guid id)
        {
            var trader = await _context.Traders
                .FirstOrDefaultAsync(t => t.Id == id);
            return trader;
        }

        public async Task<List<Trader>> GetTraders()
        {
            var traders = await _context.Traders.ToListAsync();
            return traders;
        }

        public async Task<List<Trader>> GetTradersByMineId(Guid mineId)
        {
            var traders = await _context.Traders.ToListAsync();

            var filteredTraders =
                (from t in traders
                 from c in t.CloudStocks
                 where c.MineId == mineId
                 select t)
                .ToList();

            return filteredTraders;
        }

        public async Task<Trader> SaveTrader(Trader trader)
        {
            trader.Id = new Guid();
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> SetBalance(Guid id, int balance)
        {
            var trader = await _context.Traders.FindAsync(id);
            if (trader != null)
            {
                trader.Balance = balance;
            }
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> UpdateBalance(Guid id, int amount)
        {
            var trader = await _context.Traders.FindAsync(id);
            if (trader != null)
            {
                var newBalance = trader.Balance + amount;
                trader.Balance = newBalance;
            }
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<Trader> DeleteTraderMine(Guid id, Guid mineId)
        {
            var trader = await GetTrader(id);
            var traderMine = trader.CloudStocks.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine != null)
            {
                trader.CloudStocks.Remove(traderMine);
            }
            await _context.SaveChangesAsync();
            return trader;
        }
    }
}
