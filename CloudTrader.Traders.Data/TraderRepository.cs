﻿using System.Threading.Tasks;
using CloudTrader.Traders.Service;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Data
{
    public interface ITraderRepository
    {
        Task SaveTrader(Trader trader);

        Task<Trader> GetTrader(int id);
    }

    public class TraderRepository : ITraderRepository
    {
        private readonly TraderContext _context;

        public TraderRepository()
        {
            var contextOptions = new DbContextOptionsBuilder<TraderContext>()
                .Options;
            _context = new TraderContext(contextOptions);
        }

        public async Task<Trader> GetTrader(int id)
        {
            return await _context.Traders.FindAsync(id);
        }

        public async Task SaveTrader(Trader trader)
        {
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();
        }
    }
}
