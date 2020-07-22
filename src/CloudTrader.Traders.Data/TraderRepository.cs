using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudTrader.Traders.Models.Data;
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

        public async Task<TraderDbModel> SetTraderMine(Guid id, int mineId, int stock)
        {
            var trader = await GetTrader(id);
            if (trader == null)
            {
                return trader;
            }
            trader.CloudStockDbModels ??= new List<CloudStockDbModel>();
            var existingMine = trader.CloudStockDbModels.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (existingMine == null)
            {
                trader.CloudStockDbModels.Add(new CloudStockDbModel { Id = new Guid(), MineId = mineId, Stock = stock }) ;
            } 
            else
            {
                existingMine.Stock = stock;
            }
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<TraderDbModel> GetTrader(Guid id)
        {
            var trader = await _context.Traders
                .FirstOrDefaultAsync(t => t.Id == id);
            return trader;
        }

        public async Task<List<TraderDbModel>> GetTraders()
        {
            var traders = await _context.Traders.ToListAsync();
            return traders;
        }

        public async Task<TraderDbModel> SaveTrader(TraderDbModel trader)
        {
            trader.Id = new Guid();
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<TraderDbModel> SetBalance(Guid id, int balance)
        {
            var trader = await _context.Traders.FindAsync(id);
            if (trader != null)
            {
                trader.Balance = balance;
            }
            await _context.SaveChangesAsync();
            return trader;
        }

        public async Task<TraderDbModel> DeleteTraderMine(Guid id, int mineId)
        {
            var trader = await GetTrader(id);
            var traderMine = trader.CloudStockDbModels.FirstOrDefault(cloudStock => cloudStock.MineId == mineId);
            if (traderMine != null)
            {
                trader.CloudStockDbModels.Remove(traderMine);
            }
            await _context.SaveChangesAsync();
            return trader;
        }
    }
}
