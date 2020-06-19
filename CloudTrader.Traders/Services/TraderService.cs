using System.Collections.Generic;
using System.Threading.Tasks;
using CloudTrader.Traders.Helpers;
using CloudTrader.Traders.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTrader.Traders.Services
{
    public interface ITraderService
    {
        Task<Trader> Create(Trader trader);

        Task<Trader> Update(Trader trader);

        Task<IEnumerable<Trader>> GetAll();

        Task<Trader> GetById(int id);
    }

    public class TraderService : ITraderService
    {
        private readonly DataContext _context;

        public TraderService(DataContext context)
        {
            _context = context;
        }

        public async Task<Trader> Create(Trader trader)
        {
            _context.Traders.Add(trader);
            await _context.SaveChangesAsync();

            return trader;
        }

        public async Task<Trader> Update(Trader trader)
        {
            _context.Traders.Update(trader);
            await _context.SaveChangesAsync();

            return trader;
        }

        public async Task<IEnumerable<Trader>> GetAll()
        {
            return await _context.Traders.ToListAsync();
        }

        public async Task<Trader> GetById(int id)
        {
            return await _context.Traders.FindAsync(id);
        }        
    }
}
