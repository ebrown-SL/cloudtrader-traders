using System.Threading.Tasks;
using CloudTrader.Traders.Helpers;
using CloudTrader.Traders.Service.Models;

namespace CloudTrader.Traders.Services
{
    public interface ITraderService
    {
        Task<Trader> Create(Trader trader);

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

        public async Task<Trader> GetById(int id)
        {
            return await _context.Traders.FindAsync(id);
        }        
    }
}
